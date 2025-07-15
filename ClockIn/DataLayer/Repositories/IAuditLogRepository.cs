using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface IAuditLogRepository
    {
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<AuditLog?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(AuditLog entry);
        Task<bool> UpdateAsync(AuditLog entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
