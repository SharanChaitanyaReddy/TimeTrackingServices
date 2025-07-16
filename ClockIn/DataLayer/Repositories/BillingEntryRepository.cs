using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class BillingEntryRepository : IBillingEntryRepository
    {
        private readonly DbContext _context;

        public BillingEntryRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BillingEntry>> GetAllAsync()
        {
            const string query = "SELECT * FROM billing_entries";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<BillingEntry>(query);
        }

        public async Task<BillingEntry?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM billing_entries WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<BillingEntry>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(BillingEntry billingEntry)
        {
            const string sql = @"
            INSERT INTO billing_entries (
                id, time_entry_id, user_id, hourly_rate, billable, total_amount, created_at
            )
            VALUES (
                @Id, @TimeEntryId, @UserId, @HourlyRate, @Billable, @TotalAmount, @CreatedAt
            )";

            billingEntry.Id = Guid.NewGuid();
            billingEntry.CreatedAt = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, billingEntry);
            return billingEntry.Id;
        }

        public async Task<bool> UpdateAsync(BillingEntry billingEntry)
        {
            const string sql = @"
            UPDATE billing_entries
            SET
                time_entry_id = @TimeEntryId,
                user_id = @UserId,
                hourly_rate = @HourlyRate,
                billable = @Billable,
                total_amount = @TotalAmount
            WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, billingEntry);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM billing_entries WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
