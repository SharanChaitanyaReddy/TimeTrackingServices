using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface IHolidayRepository
    {
        Task<IEnumerable<Holiday>> GetAllAsync();
        Task<Holiday?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Holiday entry);
        Task<bool> UpdateAsync(Holiday entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
