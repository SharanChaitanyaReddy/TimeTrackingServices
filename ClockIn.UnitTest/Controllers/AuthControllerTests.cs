using ClockIn.Controllers;
using ClockIn.Security.models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            var jwtSection = new Mock<IConfigurationSection>();
            jwtSection.Setup(x => x.Get<JwtConfig>()).Returns(new JwtConfig
            {
                SecretKey = "supersecretkey1234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryMinutes = 60
            });

            _mockConfig.Setup(x => x.GetSection("JwtConfig")).Returns(jwtSection.Object);

            _controller = new AuthController(_mockConfig.Object);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "P@ssw0rd"
            };

            var result = _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenObj = okResult.Value as IDictionary<string, object>;
            Assert.NotNull(tokenObj);
            Assert.True(tokenObj.ContainsKey("token"));
            Assert.IsType<string>(tokenObj["token"]);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var request = new LoginRequest
            {
                Username = "wronguser",
                Password = "wrongpassword"
            };

            var result = _controller.Login(request);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password", unauthorized.Value);
        }
    }
}