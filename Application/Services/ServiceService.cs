using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class ServiceService : IServiceService
{
   private readonly IServiceRepository _repository;
   
   public ServiceService(IServiceRepository repository)
   {
      _repository = repository;
   }

   public async Task<List<ServiceDto>> GetMainServicesAsync()
   {
      var services = await _repository.GetMainServicesAsync();
      return services.Select(s => new ServiceDto(s)).ToList();
   }
}