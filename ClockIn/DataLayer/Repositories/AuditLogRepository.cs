using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DbContext _context;

        public AuditLogRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            const string query = "SELECT * FROM audit_logs ORDER BY timestamp DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<AuditLog>(query);
        }

        public async Task<AuditLog?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM audit_logs WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AuditLog>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(AuditLog log)
        {
            const string sql = @"
            INSERT INTO audit_logs (user_id, action, entity_name, entity_id, timestamp, metadata)
            VALUES (@UserId, @Action, @EntityName, @EntityId, @Timestamp, @Metadata)";

            log.Timestamp = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, log);
            return log.Id;
        }

        // 🚫 No Update/Delete — audit logs should be immutable
    }

}
