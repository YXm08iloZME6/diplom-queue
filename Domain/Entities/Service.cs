namespace Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "Название сервиса";
    public string? Description { get; set; }
    public string? Letter { get; set; }
    public string? IconName { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsNeedFacets { get; set; } = true;
    public bool NeedMoreInfo { get; set; } = false;

    public Guid? ParentId { get; set; }
    public Service? Parent { get; set; }
    public ICollection<Service> Children { get; set; } = new List<Service>();
}