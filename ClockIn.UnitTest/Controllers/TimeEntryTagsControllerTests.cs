using ClockIn.Controllers;
using ClockIn.DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class TimeEntryTagsControllerTests
    {
        private readonly Mock<ITimeEntryTagRepository> _mockRepo;
        private readonly TimeEntryTagsController _controller;

        public TimeEntryTagsControllerTests()
        {
            _mockRepo = new Mock<ITimeEntryTagRepository>();
            _controller = new TimeEntryTagsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetTagsForTimeEntry_ReturnsOkWithTagIds()
        {
            var timeEntryId = Guid.NewGuid();
            var tagIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockRepo.Setup(r => r.GetTagIdsByTimeEntryIdAsync(timeEntryId)).ReturnsAsync(tagIds);

            var result = await _controller.GetTagsForTimeEntry(timeEntryId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tagIds, okResult.Value);
        }

        [Fact]
        public async Task AddTagToTimeEntry_ReturnsNoContent()
        {
            var timeEntryId = Guid.NewGuid();
            var tagId = Guid.NewGuid();

            _mockRepo.Setup(r => r.AddAsync(timeEntryId, tagId)).Returns(Task.CompletedTask);

            var result = await _controller.AddTagToTimeEntry(timeEntryId, tagId);

            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(r => r.AddAsync(timeEntryId, tagId), Times.Once);
        }

        [Fact]
        public async Task RemoveTagFromTimeEntry_Success_ReturnsNoContent()
        {
            var timeEntryId = Guid.NewGuid();
            var tagId = Guid.NewGuid();

            _mockRepo.Setup(r => r.RemoveAsync(timeEntryId, tagId)).ReturnsAsync(true);

            var result = await _controller.RemoveTagFromTimeEntry(timeEntryId, tagId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveTagFromTimeEntry_NotFound_ReturnsNotFound()
        {
            var timeEntryId = Guid.NewGuid();
            var tagId = Guid.NewGuid();

            _mockRepo.Setup(r => r.RemoveAsync(timeEntryId, tagId)).ReturnsAsync(false);

            var result = await _controller.RemoveTagFromTimeEntry(timeEntryId, tagId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}