using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using ClockIn.Security.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return Unauthorized("Invalid username or password");

            var user = await _userRepository.GetByUsernameAsync(request.Email);

            string hashedValue = HashPassword(request.Password);

            if (user == null || user.Email != request.Email || user.PasswordHash != hashedValue)
                return Unauthorized("Invalid username or password");

            var token = GenerateJwtToken(user);

            return Ok(new { token = token, role = user.Role });
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
            if (jwtSettings == null) throw new InvalidOperationException("JWT settings are not configured properly.");
            if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey)) throw new InvalidOperationException("JWT SecretKey is not configured.");
            if (string.IsNullOrWhiteSpace(jwtSettings.Issuer)) throw new InvalidOperationException("JWT Issuer is not configured.");
            if (string.IsNullOrWhiteSpace(jwtSettings.Audience)) throw new InvalidOperationException("JWT Audience is not configured.");
            if(string.IsNullOrWhiteSpace(jwtSettings.ExpiryMinutes.ToString())) throw new InvalidOperationException("JWT ExpiryMinutes is not configured.");
            
            var key = SHA256.HashData(Encoding.ASCII.GetBytes(jwtSettings.SecretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash);
            await _userRepository.CreateAsync(user);

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

    }

}
