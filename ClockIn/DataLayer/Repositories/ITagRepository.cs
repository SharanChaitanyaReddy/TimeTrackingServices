using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Tag entry);
        Task<bool> UpdateAsync(Tag entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
