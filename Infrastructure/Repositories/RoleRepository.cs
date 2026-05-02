using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Queue.Applications.Interfaces;
using Queue.Domain.Entities;

namespace Queue.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly QueueDbContext _context;

        public RoleRepository(QueueDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByTitleAsync(string title)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Title == title);
        }
    }
}