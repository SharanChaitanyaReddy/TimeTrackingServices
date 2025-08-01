using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/taskitems")]
    [Authorize(Roles = "Operations,Admin,Manager")]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemRepository _repository;

        public TaskItemsController(ITaskItemRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _repository.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            var id = await _repository.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TaskItem task)
        {
            if (id != task.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(task);
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
