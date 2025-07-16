using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/leaves")]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveRepository _repository;

        public LeavesController(ILeaveRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var leaves = await _repository.GetAllAsync();
            return Ok(leaves);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var leave = await _repository.GetByIdAsync(id);
            if (leave == null) return NotFound();
            return Ok(leave);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Leave leave)
        {
            var id = await _repository.CreateAsync(leave);
            return CreatedAtAction(nameof(GetById), new { id }, leave);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Leave leave)
        {
            if (id != leave.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(leave);
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
