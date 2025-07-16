using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface ITimeEntryTagRepository
    {
        Task<IEnumerable<Guid>> GetTagIdsByTimeEntryIdAsync(Guid timeEntryId);
        Task AddAsync(Guid timeEntryId, Guid tagId);
        Task<bool> RemoveAsync(Guid timeEntryId, Guid tagId);

    }
}
