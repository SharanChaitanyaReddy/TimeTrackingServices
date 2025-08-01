using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
{
    public interface IAuditLogRepository
    {
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<AuditLog?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(AuditLog entry);
    }
}
