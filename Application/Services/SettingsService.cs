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

    public async Task<SettingsDto> GetAsync()
    {
        var settings = await _repository.GetAsync();
        return new SettingsDto
        {
            Id = settings.Id,
            SimpleMode = settings.SimpleMode,
            SimpleModeServiceId = settings.SimpleModeServiceId
        };
    }
}
