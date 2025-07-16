using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly DbContext _context;

        public HolidayRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Holiday>> GetAllAsync()
        {
            const string query = "SELECT * FROM holidays ORDER BY holiday_date ASC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Holiday>(query);
        }

        public async Task<Holiday?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM holidays WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Holiday>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(Holiday holiday)
        {
            const string sql = @"
            INSERT INTO holidays (id, name, holiday_date, description, region)
            VALUES (@Id, @Name, @HolidayDate, @Description, @Region)";

            holiday.Id = Guid.NewGuid();

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, holiday);
            return holiday.Id;
        }

        public async Task<bool> UpdateAsync(Holiday holiday)
        {
            const string sql = @"
            UPDATE holidays
            SET name = @Name, holiday_date = @HolidayDate,
                description = @Description, region = @Region
            WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, holiday);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM holidays WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
