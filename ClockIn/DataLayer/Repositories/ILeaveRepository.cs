using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<Leave>> GetAllAsync();
        Task<Leave?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Leave entry);
        Task<bool> UpdateAsync(Leave entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
