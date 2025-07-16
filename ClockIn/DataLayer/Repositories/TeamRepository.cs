using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DbContext _context;

        public TeamRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            const string query = "SELECT * FROM teams";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Team>(query);
        }

        public async Task<Team?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM teams WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Team>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(Team team)
        {
            const string sql = "INSERT INTO teams (id, name, created_by) VALUES (@Id, @Name, @CreatedBy)";
            team.Id = Guid.NewGuid();

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, team);
            return team.Id;
        }

        public async Task<bool> UpdateAsync(Team team)
        {
            const string sql = "UPDATE teams SET name = @Name WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, team);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM teams WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
