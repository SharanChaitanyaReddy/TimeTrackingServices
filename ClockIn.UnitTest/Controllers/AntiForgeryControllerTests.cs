using ClockIn.Controllers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
	public class AntiForgeryControllerTests
	{
        private readonly Mock<IAntiforgery> _mockAntiforgery;
        private readonly AntiForgeryController _controller;

        public AntiForgeryControllerTests()
        {
            _mockAntiforgery = new Mock<IAntiforgery>();
            _controller = new AntiForgeryController(_mockAntiforgery.Object);
        }

        [Fact]
		public void GetToken_ReturnsOkWithToken()
		{
			// Arrange
			var expectedToken = "test-antiforgery-token";
			var tokens = new AntiforgeryTokens
			{
				RequestToken = expectedToken
			};

            _mockAntiforgery
                .Setup(a => a.GetAndStoreTokens(It.IsAny<HttpContext>()))
				.Returns(tokens);

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};

			// Act
			var result = _controller.GetToken();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var value = okResult.Value as dynamic;
			Assert.NotNull(value);
			Assert.Equal(expectedToken, value.token);
		}
	}
}