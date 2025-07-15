using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Project entry);
        Task<bool> UpdateAsync(Project entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
