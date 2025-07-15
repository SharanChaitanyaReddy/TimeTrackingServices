using ClockIn.Models;
using Dapper;

namespace ClockIn.DataLayer.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly DbContext _context;

        public TaskItemRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            const string query = "SELECT * FROM tasks";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<TaskItem>(query);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            const string query = "SELECT * FROM tasks WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TaskItem>(query, new { Id = id });
        }

        public async Task<Guid> CreateAsync(TaskItem task)
        {
            const string sql = @"
            INSERT INTO tasks (
                id, project_id, name, description, assigned_to, status, due_date, created_at
            )
            VALUES (
                @Id, @ProjectId, @Name, @Description, @AssignedTo, @Status, @DueDate, @CreatedAt
            )";

            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, task);
            return task.Id;
        }

        public async Task<bool> UpdateAsync(TaskItem task)
        {
            const string sql = @"
            UPDATE tasks
            SET
                name = @Name,
                description = @Description,
                assigned_to = @AssignedTo,
                status = @Status,
                due_date = @DueDate
            WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, task);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM tasks WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }

}
