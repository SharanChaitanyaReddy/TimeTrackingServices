using ClockIn.Controllers;
using ClockIn.DataLayer.IRepositories;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class ProjectsControllerTests
    {
        private readonly Mock<IProjectRepository> _mockRepo;
        private readonly ProjectsController _controller;

        public ProjectsControllerTests()
        {
            _mockRepo = new Mock<IProjectRepository>();
            _controller = new ProjectsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfProjects()
        {
            var projects = new List<Project> { new Project { Id = Guid.NewGuid() } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(projects, okResult.Value);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOkResult()
        {
            var project = new Project { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);

            var result = await _controller.GetById(project.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(project, okResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Project)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var project = new Project { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.CreateAsync(project)).ReturnsAsync(project.Id);

            var result = await _controller.Create(project);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtAction.ActionName);
            Assert.Equal(project, createdAtAction.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var project = new Project { Id = Guid.NewGuid() };

            var result = await _controller.Update(Guid.NewGuid(), project);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var project = new Project { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(project)).ReturnsAsync(true);

            var result = await _controller.Update(project.Id, project);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var project = new Project { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(project)).ReturnsAsync(false);

            var result = await _controller.Update(project.Id, project);

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