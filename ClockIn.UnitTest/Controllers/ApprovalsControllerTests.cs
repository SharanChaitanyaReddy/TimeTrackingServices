using ClockIn.Controllers;
using ClockIn.DataLayer.IRepositories;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class ApprovalsControllerTests
    {
        private readonly Mock<IApprovalRepository> _mockRepo;
        private readonly ApprovalsController _controller;

        public ApprovalsControllerTests()
        {
            _mockRepo = new Mock<IApprovalRepository>();
            _controller = new ApprovalsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfApprovals()
        {
            // Arrange
            var approvals = new List<Approval> { new Approval { Id = Guid.NewGuid() } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(approvals);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(approvals, okResult.Value);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var approval = new Approval { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.GetByIdAsync(approval.Id)).ReturnsAsync(approval);

            // Act
            var result = await _controller.GetById(approval.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(approval, okResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Approval)null);

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            // Arrange
            var approval = new Approval { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.CreateAsync(approval)).ReturnsAsync(approval.Id);

            // Act
            var result = await _controller.Create(approval);

            // Assert
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtAction.ActionName);
            Assert.Equal(approval, createdAtAction.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var approval = new Approval { Id = Guid.NewGuid() };

            // Act
            var result = await _controller.Update(Guid.NewGuid(), approval);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            // Arrange
            var approval = new Approval { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(approval)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(approval.Id, approval);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            // Arrange
            var approval = new Approval { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(approval)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(approval.Id, approval);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Success_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}