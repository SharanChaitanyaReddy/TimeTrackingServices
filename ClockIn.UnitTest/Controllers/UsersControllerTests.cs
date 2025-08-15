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
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _controller = new UsersController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "testuser",
                    Email = "test@example.com"
                }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetById_Found_ReturnsOkWithUser()
        {
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = id,
                FirstName = "testuser",
                Email = "test@example.com"
            };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

            var result = await _controller.GetById(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "testuser",
                Email = "test@example.com"
            };
            _mockRepo.Setup(r => r.CreateAsync(user)).ReturnsAsync(user.Id);

            var result = await _controller.Create(user);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(user, createdResult.Value);
            Assert.Equal(user.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "testuser",
                Email = "test@example.com"
            };
            _mockRepo.Setup(r => r.UpdateAsync(user)).ReturnsAsync(true);

            var result = await _controller.Update(user.Id, user);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "testuser",
                Email = "test@example.com"
            };
            _mockRepo.Setup(r => r.UpdateAsync(user)).ReturnsAsync(false);

            var result = await _controller.Update(user.Id, user);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "testuser",
                Email = "test@example.com"
            };
            var differentId = Guid.NewGuid();

            var result = await _controller.Update(differentId, user);

            Assert.IsType<BadRequestResult>(result);
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