using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/teams")]
    [Authorize(Roles = "operations,admin")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _repository;

        public TeamsController(ITeamRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _repository.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var team = await _repository.GetByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            var id = await _repository.CreateAsync(team);
            return CreatedAtAction(nameof(GetById), new { id }, team);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Team team)
        {
            if (id != team.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(team);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _repository.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }

}
