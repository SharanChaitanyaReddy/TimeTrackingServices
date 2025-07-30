using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
{
    public interface IApprovalRepository
    {
        Task<IEnumerable<Approval>> GetAllAsync();
        Task<Approval?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Approval entry);
        Task<bool> UpdateAsync(Approval entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
