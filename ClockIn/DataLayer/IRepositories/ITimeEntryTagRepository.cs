using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
{
    public interface ITimeEntryTagRepository
    {
        Task<IEnumerable<Guid>> GetTagIdsByTimeEntryIdAsync(Guid timeEntryId);
        Task AddAsync(Guid timeEntryId, Guid tagId);
        Task<bool> RemoveAsync(Guid timeEntryId, Guid tagId);

    }
}
