using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface ITimeEntryTagRepository
    {
        Task<IEnumerable<TimeEntryTag>> GetAllAsync();
        Task<TimeEntryTag?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(TimeEntryTag entry);
        Task<bool> UpdateAsync(TimeEntryTag entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
