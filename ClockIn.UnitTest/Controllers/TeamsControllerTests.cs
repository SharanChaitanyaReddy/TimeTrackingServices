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
    public class TeamsControllerTests
    {
        private readonly Mock<ITeamRepository> _mockRepo;
        private readonly TeamsController _controller;

        public TeamsControllerTests()
        {
            _mockRepo = new Mock<ITeamRepository>();
            _controller = new TeamsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithTeams()
        {
            var teams = new List<Team>
            {
                new Team
                {
                    Id = Guid.NewGuid(),
                    Name = "Team Alpha",
                    Description = "First team",
                    CreatedAt = DateTime.UtcNow
                }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(teams);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(teams, okResult.Value);
        }

        [Fact]
        public async Task GetById_Found_ReturnsOkWithTeam()
        {
            var id = Guid.NewGuid();
            var team = new Team
            {
                Id = id,
                Name = "Team Beta",
                Description = "Second team",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(team);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(team, okResult.Value);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Team?)null);

            var result = await _controller.GetById(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Team Gamma",
                Description = "Third team",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.CreateAsync(team)).ReturnsAsync(team.Id);

            var result = await _controller.Create(team);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(team, createdResult.Value);
            Assert.Equal(team.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Team Delta",
                Description = "Fourth team",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.UpdateAsync(team)).ReturnsAsync(true);

            var result = await _controller.Update(team.Id, team);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Team Epsilon",
                Description = "Fifth team",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.UpdateAsync(team)).ReturnsAsync(false);

            var result = await _controller.Update(team.Id, team);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Team Zeta",
                Description = "Sixth team",
                CreatedAt = DateTime.UtcNow
            };
            var differentId = Guid.NewGuid();

            var result = await _controller.Update(differentId, team);

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