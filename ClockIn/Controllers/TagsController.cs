using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/tags")]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _repository;

        public TagsController(ITagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _repository.GetAllAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tag = await _repository.GetByIdAsync(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            var id = await _repository.CreateAsync(tag);
            return CreatedAtAction(nameof(GetById), new { id }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Tag tag)
        {
            if (id != tag.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(tag);
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
