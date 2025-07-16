using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/holidays")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayRepository _repository;

        public HolidaysController(IHolidayRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var holidays = await _repository.GetAllAsync();
            return Ok(holidays);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var holiday = await _repository.GetByIdAsync(id);
            if (holiday == null) return NotFound();
            return Ok(holiday);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Holiday holiday)
        {
            var id = await _repository.CreateAsync(holiday);
            return CreatedAtAction(nameof(GetById), new { id }, holiday);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Holiday holiday)
        {
            if (id != holiday.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(holiday);
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
