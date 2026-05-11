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

    public async Task<List<Guid>> GetServiceTreeByIdAsync(Guid parentId)
    {
        var result = new List<Guid> { parentId };
        
        var children = await _dbContext.Set<Service>()
            .Where(s => s.ParentId == parentId)
            .Select(s => s.Id)
            .ToListAsync();

        result.AddRange(children);

        return result;
    }

    public async Task<IEnumerable<Service>> GetMainServicesAsync()
    {
        return await _dbContext.Set<Service>()
            .Where(s => s.ParentId == null)
            .ToListAsync();
    }

    public async Task<Service> GetServiceByIdAsync(Guid id)
    {
        return await _dbContext.Set<Service>()
            .Include(s => s.Children)
            .Include(s => s.Parent)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}