namespace Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "Название сервиса";
    public string? Letter { get; set; }
    
    public ICollection<Service> Parents { get; set; } = new List<Service>();
    public ICollection<Service> Children { get; set; } = new List<Service>();
}