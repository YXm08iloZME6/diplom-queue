using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SettingsRepository : ISettingsRepository
{
    private readonly QueueDbContext _dbContext;
    
    public SettingsRepository(QueueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Settings>> GetSettingsAsync()
    {
        return await _dbContext.Settings!.ToListAsync();
    }

    public async Task<Settings?> GetSettingByIdAsync(Guid id)
    {
        return await _dbContext.Settings!.Where(s => s.Id == id).FirstOrDefaultAsync();
        
    }

    public async Task<Settings?> GetSettingByNameAsync(string name)
    {
        return await _dbContext.Settings!.Where(s => s.Name == name).FirstOrDefaultAsync();
    }

    public async Task UpdateSettingsAsync(Settings settings)
    {
        _dbContext.Settings!.Update(settings);
        await _dbContext.SaveChangesAsync();
    }
}