using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IServiceService
{
    Task<List<ServiceDto>> GetMainServicesAsync();
    Task<List<ServiceDto>> GetAllServicesAsync();
    Task<ServiceDto> GetServiceByIdAsync(Guid id);
}