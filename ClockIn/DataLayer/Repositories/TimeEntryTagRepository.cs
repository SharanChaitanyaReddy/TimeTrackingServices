using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class TimeEntryTagRepository : ITimeEntryTagRepository
    {
        private readonly DbContext _context;

        public TimeEntryTagRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Guid>> GetTagIdsByTimeEntryIdAsync(Guid timeEntryId)
        {
            const string query = "SELECT tag_id FROM time_entry_tags WHERE time_entry_id = @TimeEntryId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Guid>(query, new { TimeEntryId = timeEntryId });
        }

        public async Task AddAsync(Guid timeEntryId, Guid tagId)
        {
            const string sql = "INSERT INTO time_entry_tags (time_entry_id, tag_id) VALUES (@TimeEntryId, @TagId)";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new { TimeEntryId = timeEntryId, TagId = tagId });
        }

        public async Task<bool> RemoveAsync(Guid timeEntryId, Guid tagId)
        {
            const string sql = "DELETE FROM time_entry_tags WHERE time_entry_id = @TimeEntryId AND tag_id = @TagId";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { TimeEntryId = timeEntryId, TagId = tagId });
            return result > 0;
        }
    }

}
