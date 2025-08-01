using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClockIn.Controllers
{
    [ApiController]
    [Route("api/v1/auditlogs")]
    [Authorize(Roles = "Operations,Admin,Manager")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogRepository _repository;

        public AuditLogsController(IAuditLogRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _repository.GetAllAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuditLog log)
        {
            var id = await _repository.CreateAsync(log);
            return CreatedAtAction(nameof(GetById), new { id }, log);
        }

        // 🚫 No update/delete methods due to immutability
    }

}
