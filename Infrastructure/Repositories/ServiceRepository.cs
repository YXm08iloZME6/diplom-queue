using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly QueueDbContext _dbContext;
    
    public ServiceRepository(QueueDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Service>> GetMainServicesAsync()
    {
        return await _dbContext.Set<Service>()
            .Include(s => s.Children)
            .Where(s => !s.Parents.Any())
            .ToListAsync();
    }
}