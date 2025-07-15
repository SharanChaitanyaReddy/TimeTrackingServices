namespace ClockIn.DataLayer.Repositories
{
    using ClockIn.Models;
    using Dapper;

    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly DbContext _context;

        public TimeEntryRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeEntry>> GetAllAsync()
        {
            const string query = "SELECT * FROM time_entries";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<TimeEntry>(query);
        }

        public async Task<TimeEntry?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM time_entries WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TimeEntry>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(TimeEntry entry)
        {
            const string sql = @"
            INSERT INTO time_entries (
                id, user_id, task_id, project_id,
                start_time, end_time, is_manual, description,
                is_approved, submitted_at, approved_at, approved_by, created_at
            )
            VALUES (
                @Id, @UserId, @TaskId, @ProjectId,
                @StartTime, @EndTime, @IsManual, @Description,
                @IsApproved, @SubmittedAt, @ApprovedAt, @ApprovedBy, @CreatedAt
            )";

            entry.Id = Guid.NewGuid();
            entry.CreatedAt = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, entry);
            return entry.Id;
        }

        public async Task<bool> UpdateAsync(TimeEntry entry)
        {
            const string sql = @"
            UPDATE time_entries
            SET 
                user_id = @UserId,
                task_id = @TaskId,
                project_id = @ProjectId,
                start_time = @StartTime,
                end_time = @EndTime,
                is_manual = @IsManual,
                description = @Description,
                is_approved = @IsApproved,
                submitted_at = @SubmittedAt,
                approved_at = @ApprovedAt,
                approved_by = @ApprovedBy
            WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(sql, entry);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM time_entries WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }

}
