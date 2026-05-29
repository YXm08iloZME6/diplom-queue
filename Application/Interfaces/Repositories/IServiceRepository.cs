using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetMainServicesAsync();
    Task<IEnumerable<Service>> GetAllServicesAsync();
    Task<Service> GetServiceByIdAsync(Guid id);
    Task<List<Guid>> GetServiceTreeByIdAsync(Guid parentId);
    Task CreateServiceAsync(Service service);
    Task UpdateServiceAsync(Service service);
    Task DeleteServiceAsync(Service service);
    Task SaveChangeAsync();

}