using ClockIn.Security.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        // You can inject your user repository or user manager here for user validation

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // TODO: Validate user credentials (replace with real user validation)
            if (request.Username != "testuser" || request.Password != "P@ssw0rd")
                return Unauthorized("Invalid username or password");

            var token = GenerateJwtToken(request.Username);

            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username),
                // add other claims like roles here if needed
            }),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
