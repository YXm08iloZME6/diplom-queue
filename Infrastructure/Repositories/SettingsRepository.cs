using Application.Interfaces.Repositories;
using Domain.Entities;
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

    public async Task<Settings> GetAsync()
    {
        var settings = await _dbContext.Settings!.FirstOrDefaultAsync();
        return settings ?? new Settings();
    }

    public async Task UpdateAsync(Settings settings)
    {
        _dbContext.Settings!.Update(settings);
        await _dbContext.SaveChangesAsync();
    }
}
