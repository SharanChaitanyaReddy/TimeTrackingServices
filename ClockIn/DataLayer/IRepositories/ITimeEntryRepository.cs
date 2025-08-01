using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
{
    public interface ITimeEntryRepository
    {
        Task<IEnumerable<TimeEntry>> GetAllAsync();
        Task<TimeEntry?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(TimeEntry entry);
        Task<bool> UpdateAsync(TimeEntry entry);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> IsHolidayOrWeekend(DateTime date);
    }
}
