using ClockIn.Controllers;
using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using ClockIn.Security.models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthController _controller;
        private readonly Mock<IUserRepository> _mockUserRepository;
        public AuthControllerTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockUserRepository = new Mock<IUserRepository>();

            var inMemorySettings = new Dictionary<string, string?>
{
    {"JwtConfig:SecretKey", "supersecretkey1234567890"},
    {"JwtConfig:Issuer", "TestIssuer"},
    {"JwtConfig:Audience", "TestAudience"},
    {"JwtConfig:ExpiryMinutes", "60"}
};

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();


            _controller = new AuthController(configuration, _mockUserRepository.Object);
        }
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            var request = new LoginRequest
            {
                Email = "testuser",
                Password = "hashedpassword"
            };

            _mockUserRepository.Setup(x => x.GetByUsernameAsync(request.Email))
                .ReturnsAsync(new User
                {
                    Email = request.Email,
                    PasswordHash = HashPassword("hashedpassword"), // This should match the hash of "P@ssw0rd"
                    Role = "User"
                });

            var result = await _controller.LoginAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var jObject = JObject.FromObject(okResult.Value);
            string token = jObject["token"]?.ToString();
            string role = jObject["role"]?.ToString();
            Assert.NotNull(token);
            Assert.NotNull(role);

        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var request = new LoginRequest
            {
                Email = "wronguser",
                Password = "wrongpassword"
            };

            var result = await _controller.LoginAsync(request);


            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password", unauthorized.Value);
        }
    }
}