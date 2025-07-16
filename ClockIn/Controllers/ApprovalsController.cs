using ClockIn.DataLayer.Repositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/approvals")]
    public class ApprovalsController : ControllerBase
    {
        private readonly IApprovalRepository _repository;

        public ApprovalsController(IApprovalRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var approvals = await _repository.GetAllAsync();
            return Ok(approvals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var approval = await _repository.GetByIdAsync(id);
            if (approval == null) return NotFound();
            return Ok(approval);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Approval approval)
        {
            var id = await _repository.CreateAsync(approval);
            return CreatedAtAction(nameof(GetById), new { id }, approval);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Approval approval)
        {
            if (id != approval.Id) return BadRequest("ID mismatch");
            var success = await _repository.UpdateAsync(approval);
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
