using ClockIn.Controllers;
using ClockIn.DataLayer.IRepositories;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class LeavesControllerTests
    {
        private readonly Mock<ILeaveRepository> _mockRepo;
        private readonly LeavesController _controller;

        public LeavesControllerTests()
        {
            _mockRepo = new Mock<ILeaveRepository>();
            _controller = new LeavesController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfLeaves()
        {
            var leaves = new List<Leave> { new Leave { Id = Guid.NewGuid() } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(leaves);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(leaves, okResult.Value);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOkResult()
        {
            var leave = new Leave { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.GetByIdAsync(leave.Id)).ReturnsAsync(leave);

            var result = await _controller.GetById(leave.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(leave, okResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Leave)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var leave = new Leave { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.CreateAsync(leave)).ReturnsAsync(leave.Id);

            var result = await _controller.Create(leave);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtAction.ActionName);
            Assert.Equal(leave, createdAtAction.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var leave = new Leave { Id = Guid.NewGuid() };

            var result = await _controller.Update(Guid.NewGuid(), leave);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var leave = new Leave { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(leave)).ReturnsAsync(true);

            var result = await _controller.Update(leave.Id, leave);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var leave = new Leave { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(leave)).ReturnsAsync(false);

            var result = await _controller.Update(leave.Id, leave);

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