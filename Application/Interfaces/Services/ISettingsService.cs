using Application.DTOs;

namespace Application.Interfaces.Services;

public interface ISettingsService
{
    Task<SettingsDto> GetAsync();
}
