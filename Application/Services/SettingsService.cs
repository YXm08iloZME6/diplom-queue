using System;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services;

public class SettingsService : ISettingsService
{
    private readonly ISettingsRepository _repository;

    public SettingsService(ISettingsRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<List<SettingsDto>> GetSettingsAsync()
    {
        var settings = await _repository.GetSettingsAsync();
        
        return settings.Select(s => new SettingsDto(s)).ToList();
    }

    public async Task<SettingsDto?> GetSettingByIdAsync(Guid id)
    {
        var settings = await _repository.GetSettingByIdAsync(id);
        
        return settings != null ? new SettingsDto(settings) : null;
    }

    public async Task<SettingsDto?> GetSettingByNameAsync(string name)
    {
        var settings = await _repository.GetSettingByNameAsync(name);
        return settings != null ? new SettingsDto(settings) : null;
    }

    public async Task UpdateSettingValueAsync(Guid id, string value)
    {
        await _repository.UpdateSettingValueAsync(id, value);
    }
}