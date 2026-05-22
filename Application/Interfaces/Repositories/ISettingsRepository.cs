using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISettingsRepository
{
    Task<Settings> GetAsync();
    Task UpdateAsync(Settings settings);
}
