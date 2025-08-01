using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
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
