using ClockIn.Controllers;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class TagsControllerTests
    {
        private readonly Mock<ITagRepository> _mockRepo;
        private readonly TagsController _controller;

        public TagsControllerTests()
        {
            _mockRepo = new Mock<ITagRepository>();
            _controller = new TagsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfTags()
        {
            var tags = new List<Tag> { new Tag { Id = Guid.NewGuid() } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tags);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tags, okResult.Value);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOkResult()
        {
            var tag = new Tag { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.GetByIdAsync(tag.Id)).ReturnsAsync(tag);

            var result = await _controller.GetById(tag.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tag, okResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Tag)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var tag = new Tag { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.CreateAsync(tag)).ReturnsAsync(tag.Id);

            var result = await _controller.Create(tag);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtAction.ActionName);
            Assert.Equal(tag, createdAtAction.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var tag = new Tag { Id = Guid.NewGuid() };

            var result = await _controller.Update(Guid.NewGuid(), tag);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var tag = new Tag { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(tag)).ReturnsAsync(true);

            var result = await _controller.Update(tag.Id, tag);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var tag = new Tag { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateAsync(tag)).ReturnsAsync(false);

            var result = await _controller.Update(tag.Id, tag);

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