using ClockIn.Controllers;
using ClockIn.DataLayer.IRepositories;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClockIn.UnitTest.Controllers
{
    public class ProductivityControllerTests
    {
        private readonly Mock<IProductivityRepository> _mockRepo;
        private readonly ProductivityController _controller;

        public ProductivityControllerTests()
        {
            _mockRepo = new Mock<IProductivityRepository>();
            _controller = new ProductivityController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithBenchmarks()
        {
            var benchmarks = new List<ProductivityBenchmark>
            {
                new ProductivityBenchmark
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    WeekStart = DateTime.UtcNow,
                    ExpectedHours = 40,
                    ActualHours = 38,
                    Status = "On Track",
                    CreatedAt = DateTime.UtcNow
                }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(benchmarks);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(benchmarks, okResult.Value);
        }

        [Fact]
        public async Task GetById_Found_ReturnsOkWithBenchmark()
        {
            var id = Guid.NewGuid();
            var benchmark = new ProductivityBenchmark
            {
                Id = id,
                UserId = Guid.NewGuid(),
                WeekStart = DateTime.UtcNow,
                ExpectedHours = 40,
                ActualHours = 38,
                Status = "On Track",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(benchmark);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(benchmark, okResult.Value);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ProductivityBenchmark?)null);

            var result = await _controller.GetById(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var benchmark = new ProductivityBenchmark
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WeekStart = DateTime.UtcNow,
                ExpectedHours = 40,
                ActualHours = 38,
                Status = "On Track",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.CreateAsync(benchmark)).ReturnsAsync(benchmark.Id);

            var result = await _controller.Create(benchmark);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(benchmark, createdResult.Value);
            Assert.Equal(benchmark.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var benchmark = new ProductivityBenchmark
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WeekStart = DateTime.UtcNow,
                ExpectedHours = 40,
                ActualHours = 38,
                Status = "On Track",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.UpdateAsync(benchmark)).ReturnsAsync(true);

            var result = await _controller.Update(benchmark.Id, benchmark);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var benchmark = new ProductivityBenchmark
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WeekStart = DateTime.UtcNow,
                ExpectedHours = 40,
                ActualHours = 38,
                Status = "On Track",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.UpdateAsync(benchmark)).ReturnsAsync(false);

            var result = await _controller.Update(benchmark.Id, benchmark);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var benchmark = new ProductivityBenchmark
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WeekStart = DateTime.UtcNow,
                ExpectedHours = 40,
                ActualHours = 38,
                Status = "On Track",
                CreatedAt = DateTime.UtcNow
            };
            var differentId = Guid.NewGuid();

            var result = await _controller.Update(differentId, benchmark);

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