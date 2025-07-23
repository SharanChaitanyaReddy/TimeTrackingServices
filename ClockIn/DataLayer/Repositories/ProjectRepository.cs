using ClockIn.DataLayer.IRepositories;
using ClockIn.Models;

namespace ClockIn.DataLayer.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DbContext _context;

        public ProjectRepository(DbContext context)
        {
            _context = context;
        }
        public Task<Guid> CreateAsync(Project entry)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Project?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Project entry)
        {
            throw new NotImplementedException();
        }
    }
}
