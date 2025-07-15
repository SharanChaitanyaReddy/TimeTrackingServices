using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var sql = "SELECT * FROM users";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM users WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<Guid> CreateAsync(User user)
        {
            var sql = @"
            INSERT INTO users (id, name, email, role, team_id, hourly_rate, is_active, created_at)
            VALUES (@Id, @Name, @Email, @Role, @TeamId, @HourlyRate, @IsActive, @CreatedAt)";
            using var connection = _context.CreateConnection();
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            await connection.ExecuteAsync(sql, user);
            return user.Id;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var sql = @"
            UPDATE users
            SET name = @Name,
                email = @Email,
                role = @Role,
                team_id = @TeamId,
                hourly_rate = @HourlyRate,
                is_active = @IsActive
            WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(sql, user);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sql = "DELETE FROM users WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
