using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DbContext _context;

        public TagRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            const string query = "SELECT * FROM tags";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Tag>(query);
        }

        public async Task<Tag?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM tags WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Tag>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(Tag tag)
        {
            const string sql = "INSERT INTO tags (name) VALUES (@Name)";
            tag.Id = Guid.NewGuid();

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, tag);
            return tag.Id;
        }

        public async Task<bool> UpdateAsync(Tag tag)
        {
            const string sql = "UPDATE tags SET name = @Name WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, tag);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM tags WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
