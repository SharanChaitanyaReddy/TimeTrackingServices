using ClockIn.Controllers;
using ClockIn.DataLayer.IRepositories;
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
    public class TaskItemsControllerTests
    {
        private readonly Mock<ITaskItemRepository> _mockRepo;
        private readonly TaskItemsController _controller;

        public TaskItemsControllerTests()
        {
            _mockRepo = new Mock<ITaskItemRepository>();
            _controller = new TaskItemsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithTasks()
        {
            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    ProjectId = Guid.NewGuid(),
                    Name = "Test Task",
                    Description = "Test Description",
                    AssignedTo = Guid.NewGuid(),
                    Status = "Open",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow
                }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tasks, okResult.Value);
        }

        [Fact]
        public async Task GetById_Found_ReturnsOkWithTask()
        {
            var id = Guid.NewGuid();
            var task = new TaskItem
            {
                Id = id,
                ProjectId = Guid.NewGuid(),
                Name = "Test Task",
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                Status = "Open",
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(task);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(task, okResult.Value);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);

            var result = await _controller.GetById(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Name = "Test Task",
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                Status = "Open",
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.CreateAsync(task)).ReturnsAsync(task.Id);

            var result = await _controller.Create(task);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(task, createdResult.Value);
            Assert.Equal(task.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Name = "Test Task",
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                Status = "Open",
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.UpdateAsync(task)).ReturnsAsync(true);

            var result = await _controller.Update(task.Id, task);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Name = "Test Task",
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                Status = "Open",
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.UpdateAsync(task)).ReturnsAsync(false);

            var result = await _controller.Update(task.Id, task);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Name = "Test Task",
                Description = "Test Description",
                AssignedTo = Guid.NewGuid(),
                Status = "Open",
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            var differentId = Guid.NewGuid();

            var result = await _controller.Update(differentId, task);

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