using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/Projects")]
    [Authorize(Roles = "operations,admin,manager")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _repository;

        public ProjectsController(IProjectRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _repository.GetAllAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await _repository.GetByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            var id = await _repository.CreateAsync(project);
            return CreatedAtAction(nameof(GetById), new { id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Project project)
        {
            if (id != project.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(project);
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
