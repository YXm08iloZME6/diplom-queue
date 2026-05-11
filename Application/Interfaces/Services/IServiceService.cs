using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IServiceService
{
    Task<List<ServiceDto>> GetMainServicesAsync();
    Task<ServiceDto> GetServiceByIdAsync(Guid id);
    Task<ServiceDto> AddServiceAsync(CreateServiceDto serviceDto);
}