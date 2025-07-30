using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/billingentries")]
    [Authorize(Roles = "Admin")]
    public class BillingEntriesController : ControllerBase
    {
        private readonly IBillingEntryRepository _repository;

        public BillingEntriesController(IBillingEntryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _repository.GetAllAsync();
            return Ok(entries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entry = await _repository.GetByIdAsync(id);
            if (entry == null) return NotFound();
            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BillingEntry entry)
        {
            var id = await _repository.CreateAsync(entry);
            return CreatedAtAction(nameof(GetById), new { id }, entry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, BillingEntry entry)
        {
            if (id != entry.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(entry);
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
