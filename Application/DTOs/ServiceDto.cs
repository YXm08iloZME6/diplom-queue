using Domain.Entities;

namespace Application.DTOs;

public class ServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string? Letter { get; set; }
    public List<ServiceDto> Children { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public bool IsActive { get; set; }
    public bool IsNeedFacets { get; set; }
    public bool NeedMoreInfo { get; set; }
    public ServiceDto(Service service)
    {
        Id = service.Id;
        Name = service.Name;
        Letter = service.Letter;
        Children = service.Children.Select(x => new ServiceDto(x)).ToList();
        Description = service.Description;
        IconName = service.IconName;
        IsActive = service.IsActive;
        IsNeedFacets = service.IsNeedFacets;
        NeedMoreInfo = service.NeedMoreInfo;
    }
}

public class CreateServiceDto
{
    public string Name { get; set; }
    public string Letter { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
}




