using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface ISettingsService
{
    public Task<List<SettingsDto>> GetSettingsAsync();
    public Task<SettingsDto?> GetSettingByIdAsync(Guid id);
    public Task<SettingsDto?> GetSettingByNameAsync(string name);
    public Task UpdateSettingsAsync(SettingsDto settings);
    public Task UpdateSettingValueAsync(Guid id, string value);
}