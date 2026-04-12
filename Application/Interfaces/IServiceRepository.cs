using Domain.Entities;

namespace Application.Interfaces;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetMainServicesAsync();
    Task<Service> GetServiceByIdAsync(Guid id);
}