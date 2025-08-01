using ClockIn.Controllers;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class AuditLogsControllerTests
    {
        private readonly Mock<IAuditLogRepository> _mockRepo;
        private readonly AuditLogsController _controller;

        public AuditLogsControllerTests()
        {
            _mockRepo = new Mock<IAuditLogRepository>();
            _controller = new AuditLogsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithLogs()
        {
            // Arrange
            var logs = new List<AuditLog>
            {
                new AuditLog { Id = Guid.NewGuid(), Action = "Test", UserId = Guid.NewGuid(), ResourceType = "Type", ResourceId = Guid.NewGuid(), Details = null, Timestamp = DateTime.UtcNow }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(logs);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(logs, okResult.Value);
        }

        [Fact]
        public async Task GetById_Found_ReturnsOkWithLog()
        {
            // Arrange
            var id = Guid.NewGuid();
            var log = new AuditLog { Id = id, Action = "Test", UserId = Guid.NewGuid(), ResourceType = "Type", ResourceId = Guid.NewGuid(), Details = null, Timestamp = DateTime.UtcNow };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(log);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(log, okResult.Value);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((AuditLog?)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            // Arrange
            var log = new AuditLog { Id = Guid.NewGuid(), Action = "Create", UserId = Guid.NewGuid(), ResourceType = "Type", ResourceId = Guid.NewGuid(), Details = null, Timestamp = DateTime.UtcNow };
            _mockRepo.Setup(r => r.CreateAsync(log)).ReturnsAsync(log.Id);

            // Act
            var result = await _controller.Create(log);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(log, createdResult.Value);
            Assert.Equal(log.Id, createdResult.RouteValues["id"]);
        }
    }
}