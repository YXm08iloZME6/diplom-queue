using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface ISettingsRepository
{
    public Task<List<Settings>> GetSettingsAsync();
    public Task<Settings?> GetSettingByIdAsync(Guid id);
    public Task<Settings?> GetSettingByNameAsync(string name);
    Task UpdateSettingValueAsync(Guid id, string value);
    public Task AddSettingsAsync(Settings settings);
    public Task UpdateSettingsAsync(Settings settings);
    public Task DeleteSettingsAsync(Guid id);
}