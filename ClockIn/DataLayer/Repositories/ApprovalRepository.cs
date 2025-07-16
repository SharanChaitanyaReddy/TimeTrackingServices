using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly DbContext _context;

        public ApprovalRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Approval>> GetAllAsync()
        {
            const string query = "SELECT * FROM approvals";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Approval>(query);
        }

        public async Task<Approval?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM approvals WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Approval>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(Approval approval)
        {
            const string sql = @"
            INSERT INTO approvals (
                id, time_entry_id, status, comment, approved_by, approved_at, created_at
            )
            VALUES (
                @Id, @TimeEntryId, @Status, @Comment, @ApprovedBy, @ApprovedAt, @CreatedAt
            )";

            approval.Id = Guid.NewGuid();
            approval.CreatedAt = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, approval);
            return approval.Id;
        }

        public async Task<bool> UpdateAsync(Approval approval)
        {
            const string sql = @"
            UPDATE approvals
            SET
                time_entry_id = @TimeEntryId,
                status = @Status,
                comment = @Comment,
                approved_by = @ApprovedBy,
                approved_at = @ApprovedAt
            WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, approval);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM approvals WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
