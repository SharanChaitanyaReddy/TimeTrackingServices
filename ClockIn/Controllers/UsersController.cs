using Microsoft.AspNetCore.Mvc;
using ClockIn.Models;
using Microsoft.AspNetCore.Authorization;
using ClockIn.DataLayer.IRepositories;

namespace ClockIn.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var id = await _repo.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, User user)
        {
            if (id != user.Id) return BadRequest();
            var success = await _repo.UpdateAsync(user);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _repo.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }

}
