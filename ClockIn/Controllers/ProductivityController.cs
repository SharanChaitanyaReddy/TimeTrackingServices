using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/productivity")]
    public class ProductivityController : ControllerBase
    {
        private readonly IProductivityRepository _repository;

        public ProductivityController(IProductivityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var benchmarks = await _repository.GetAllAsync();
            return Ok(benchmarks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductivityBenchmark benchmark)
        {
            var id = await _repository.CreateAsync(benchmark);
            return CreatedAtAction(nameof(GetById), new { id }, benchmark);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProductivityBenchmark benchmark)
        {
            if (id != benchmark.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(benchmark);
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
