using Application.DTOs;

namespace Application.Interfaces;

public interface IServiceService
{
    Task<List<ServiceDto>> GetMainServicesAsync();
    Task<ServiceDto> GetServiceByIdAsync(Guid id);
}