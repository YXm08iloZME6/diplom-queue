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

   public async Task<ServiceDto> GetServiceByIdAsync(Guid id)
   {
      var service = await _repository.GetServiceByIdAsync(id);
      var dto = new ServiceDto(service);

      if (dto.Letter == null)
         dto.Letter = service.Parent?.Letter;

      foreach (var child in dto.Children)
         child.Letter = dto.Letter;

      return dto;
   }
}