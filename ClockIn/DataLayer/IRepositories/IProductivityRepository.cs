using ClockIn.Models;

namespace ClockIn.DataLayer.IRepositories
{
    public interface IProductivityRepository
    {
        Task<IEnumerable<ProductivityBenchmark>> GetAllAsync();
        Task<ProductivityBenchmark?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ProductivityBenchmark entry);
        Task<bool> UpdateAsync(ProductivityBenchmark entry);
        Task<bool> DeleteAsync(Guid id);
    }
}
