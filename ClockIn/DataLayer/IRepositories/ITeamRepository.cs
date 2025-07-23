using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
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
