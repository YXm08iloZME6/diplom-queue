using System.Transactions;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ISettingsRepository _settingsRepository;

    public ServiceService(IServiceRepository serviceRepository, ISettingsRepository settingsRepository)
    {
        _serviceRepository = serviceRepository;
        _settingsRepository = settingsRepository;
    }

    public async Task<List<ServiceDto>> GetMainServicesAsync()
    {
        var services = await _serviceRepository.GetMainServicesAsync();
        return services.Select(s => new ServiceDto(s)).ToList();
    }

    public async Task<List<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllServicesAsync();
        return services.Select(s => new ServiceDto(s)).ToList();
    }

    public async Task<ServiceDto> GetServiceByIdAsync(Guid id)
    {
        var service = await _serviceRepository.GetServiceByIdAsync(id);
        var dto = new ServiceDto(service);

        if (dto.Letter == null)
            dto.Letter = service.Parent?.Letter;

        foreach (var child in dto.Children)
            child.Letter = dto.Letter;

        return dto;
    }
}