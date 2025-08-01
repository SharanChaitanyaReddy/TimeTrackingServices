using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly DbContext _context;

        public LeaveRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Leave>> GetAllAsync()
        {
            const string query = "SELECT * FROM leaves";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Leave>(query);
        }

        public async Task<Leave?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM leaves WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Leave>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(Leave leave)
        {
            const string sql = @"
            INSERT INTO leaves (
                user_id, start_date, end_date, reason, leave_type, status, created_at
            )
            VALUES (
                @UserId, @StartDate, @EndDate, @Reason, @LeaveType, @Status, @CreatedAt
            )";
            leave.CreatedAt = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, leave);
            return leave.Id;
        }

        public async Task<bool> UpdateAsync(Leave leave)
        {
            const string sql = @"
            UPDATE leaves
            SET start_date = @StartDate, end_date = @EndDate,
                reason = @Reason, leave_type = @LeaveType, status = @Status
            WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, leave);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM leaves WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
