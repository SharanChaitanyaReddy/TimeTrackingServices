using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Team entry);
        Task<bool> UpdateAsync(Team entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
