using ClockIn.DataLayer.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/timeentrytags")]
    public class TimeEntryTagsController : ControllerBase
    {
        private readonly ITimeEntryTagRepository _repository;

        public TimeEntryTagsController(ITimeEntryTagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{timeEntryId}")]
        public async Task<IActionResult> GetTagsForTimeEntry(Guid timeEntryId)
        {
            var tagIds = await _repository.GetTagIdsByTimeEntryIdAsync(timeEntryId);
            return Ok(tagIds);
        }

        [HttpPost]
        public async Task<IActionResult> AddTagToTimeEntry(Guid timeEntryId, Guid tagId)
        {
            await _repository.AddAsync(timeEntryId, tagId);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTagFromTimeEntry(Guid timeEntryId, Guid tagId)
        {
            var result = await _repository.RemoveAsync(timeEntryId, tagId);
            return result ? NoContent() : NotFound();
        }
    }

}
