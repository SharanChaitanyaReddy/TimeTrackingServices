using ClockIn.Controllers;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class BillingEntriesControllerTests
    {
        private readonly Mock<IBillingEntryRepository> _mockRepo;
        private readonly BillingEntriesController _controller;

        public BillingEntriesControllerTests()
        {
            _mockRepo = new Mock<IBillingEntryRepository>();
            _controller = new BillingEntriesController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithEntries()
        {
            var entries = new List<BillingEntry>
            {
                new BillingEntry { Id = Guid.NewGuid(), TimeEntryId = Guid.NewGuid(), UserId = Guid.NewGuid(), HourlyRate = 100, Billable = true, TotalAmount = 200, CreatedAt = DateTime.UtcNow }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(entries);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(entries, okResult.Value);
        }

        [Fact]
        public async Task GetById_Found_ReturnsOkWithEntry()
        {
            var id = Guid.NewGuid();
            var entry = new BillingEntry { Id = id, TimeEntryId = Guid.NewGuid(), UserId = Guid.NewGuid(), HourlyRate = 100, Billable = true, TotalAmount = 200, CreatedAt = DateTime.UtcNow };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entry);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(entry, okResult.Value);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((BillingEntry?)null);

            var result = await _controller.GetById(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var entry = new BillingEntry { Id = Guid.NewGuid(), TimeEntryId = Guid.NewGuid(), UserId = Guid.NewGuid(), HourlyRate = 100, Billable = true, TotalAmount = 200, CreatedAt = DateTime.UtcNow };
            _mockRepo.Setup(r => r.CreateAsync(entry)).ReturnsAsync(entry.Id);

            var result = await _controller.Create(entry);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(entry, createdResult.Value);
            Assert.Equal(entry.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var entry = new BillingEntry { Id = Guid.NewGuid(), TimeEntryId = Guid.NewGuid(), UserId = Guid.NewGuid(), HourlyRate = 100, Billable = true, TotalAmount = 200, CreatedAt = DateTime.UtcNow };
            _mockRepo.Setup(r => r.UpdateAsync(entry)).ReturnsAsync(true);

            var result = await _controller.Update(entry.Id, entry);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var entry = new BillingEntry { Id = Guid.NewGuid(), TimeEntryId = Guid.NewGuid(), UserId = Guid.NewGuid(), HourlyRate = 100, Billable = true, TotalAmount = 200, CreatedAt = DateTime.UtcNow };
            _mockRepo.Setup(r => r.UpdateAsync(entry)).ReturnsAsync(false);

            var result = await _controller.Update(entry.Id, entry);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var entry = new BillingEntry { Id = Guid.NewGuid(), TimeEntryId = Guid.NewGuid(), UserId = Guid.NewGuid(), HourlyRate = 100, Billable = true, TotalAmount = 200, CreatedAt = DateTime.UtcNow };
            var differentId = Guid.NewGuid();

            var result = await _controller.Update(differentId, entry);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
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