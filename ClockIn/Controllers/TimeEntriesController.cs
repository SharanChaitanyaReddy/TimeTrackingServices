using ClockIn.DataLayer;
using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/timeentries")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryRepository _repository;
        private readonly ILogger<TimeEntriesController> _logger;

        public TimeEntriesController(ITimeEntryRepository repository, ILogger<TimeEntriesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entry = await _repository.GetByIdAsync(id);
            return entry is null ? NotFound() : Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TimeEntry entry)
        {
            if (await IsHolidayOrWeekend(entry.StartTime))
            {
                return BadRequest("You cannot submit a time entry on weekends or holidays.");
            }
            var id = await _repository.CreateAsync(entry);
            return CreatedAtAction(nameof(GetById), new { id }, entry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TimeEntry entry)
        {
            if (id != entry.Id) return BadRequest();
            if (await IsHolidayOrWeekend(entry.StartTime))
            {
                return BadRequest("You cannot submit a time entry on weekends or holidays.");
            }
            var result = await _repository.UpdateAsync(entry);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _repository.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
        private async Task<bool> IsHolidayOrWeekend(DateTime date)
        {
            /*var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
            if (isWeekend) return true;

            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(1) FROM holidays WHERE holiday_date = @Date";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Date = date.Date });
            return count > 0;*/
            return true;
        }

    }
}
