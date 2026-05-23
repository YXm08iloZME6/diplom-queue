using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface ISettingsRepository
{
    public Task<List<Settings>> GetSettingsAsync();
    public Task<Settings?> GetSettingByIdAsync(Guid id);
    public Task<Settings?> GetSettingByNameAsync(string name);
    public Task UpdateSettingsAsync(Settings settings);
}