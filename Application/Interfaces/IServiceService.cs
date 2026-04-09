using Application.DTOs;

namespace Application.Interfaces;

public interface IServiceService
{
    Task<List<ServiceDto>> GetMainServicesAsync();
}