using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class ProductivityRepository : IProductivityRepository
    {
        private readonly DbContext _context;

        public ProductivityRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductivityBenchmark>> GetAllAsync()
        {
            const string query = "SELECT * FROM productivity_benchmarks";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductivityBenchmark>(query);
        }

        public async Task<ProductivityBenchmark?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM productivity_benchmarks WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ProductivityBenchmark>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(ProductivityBenchmark benchmark)
        {
            const string sql = @"
            INSERT INTO productivity_benchmarks (
                user_id, week_start, expected_hours, actual_hours, status, created_at
            )
            VALUES (
                @UserId, @WeekStart, @ExpectedHours, @ActualHours, @Status, @CreatedAt
            )";

            benchmark.Id = Guid.NewGuid();
            benchmark.CreatedAt = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, benchmark);
            return benchmark.Id;
        }

        public async Task<bool> UpdateAsync(ProductivityBenchmark benchmark)
        {
            const string sql = @"
            UPDATE productivity_benchmarks
            SET
                user_id = @UserId,
                week_start = @WeekStart,
                expected_hours = @ExpectedHours,
                actual_hours = @ActualHours,
                status = @Status
            WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, benchmark);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM productivity_benchmarks WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
