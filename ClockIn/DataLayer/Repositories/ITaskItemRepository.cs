using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(TaskItem entry);
        Task<bool> UpdateAsync(TaskItem entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
