using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
{
    public interface IBillingEntryRepository
    {
        Task<IEnumerable<BillingEntry>> GetAllAsync();
        Task<BillingEntry?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(BillingEntry entry);
        Task<bool> UpdateAsync(BillingEntry entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
