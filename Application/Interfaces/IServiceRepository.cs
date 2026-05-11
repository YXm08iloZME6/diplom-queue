using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetMainServicesAsync();
    Task<Service> GetServiceByIdAsync(Guid id);
    Task<List<Guid>> GetServiceTreeByIdAsync(Guid parentId);
    Task CreateServiceAsync(Service service);
    Task SaveChangeAsync();
}