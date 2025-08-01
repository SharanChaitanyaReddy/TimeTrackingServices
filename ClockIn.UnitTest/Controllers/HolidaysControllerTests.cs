using ClockIn.Controllers;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class HolidaysControllerTests
    {
        private readonly Mock<IHolidayRepository> _mockRepo;
        private readonly HolidaysController _controller;

        public HolidaysControllerTests()
        {
            _mockRepo = new Mock<IHolidayRepository>();
            _controller = new HolidaysController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithHolidays()
        {
            var holidays = new List<Holiday>
            {
                new Holiday { Id = Guid.NewGuid(), Name = "Test", HolidayDate = DateTime.Today }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(holidays);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(holidays, okResult.Value);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOkWithHoliday()
        {
            var holiday = new Holiday { Id = Guid.NewGuid(), Name = "Test", HolidayDate = DateTime.Today };
            _mockRepo.Setup(r => r.GetByIdAsync(holiday.Id)).ReturnsAsync(holiday);

            var result = await _controller.GetById(holiday.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(holiday, okResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Holiday)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidHoliday_ReturnsCreatedAtAction()
        {
            var holiday = new Holiday { Id = Guid.NewGuid(), Name = "Test", HolidayDate = DateTime.Today };
            _mockRepo.Setup(r => r.CreateAsync(holiday)).ReturnsAsync(holiday.Id);

            var result = await _controller.Create(holiday);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtAction.ActionName);
            Assert.Equal(holiday, createdAtAction.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var holiday = new Holiday { Id = Guid.NewGuid(), Name = "Test", HolidayDate = DateTime.Today };

            var result = await _controller.Update(Guid.NewGuid(), holiday);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var holiday = new Holiday { Id = Guid.NewGuid(), Name = "Test", HolidayDate = DateTime.Today };
            _mockRepo.Setup(r => r.UpdateAsync(holiday)).ReturnsAsync(true);

            var result = await _controller.Update(holiday.Id, holiday);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var holiday = new Holiday { Id = Guid.NewGuid(), Name = "Test", HolidayDate = DateTime.Today };
            _mockRepo.Setup(r => r.UpdateAsync(holiday)).ReturnsAsync(false);

            var result = await _controller.Update(holiday.Id, holiday);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Success_ReturnsNoContent()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _controller.Delete(id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _controller.Delete(id);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}