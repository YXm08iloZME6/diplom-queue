namespace Domain.Entities;

public class Settings
{
    public Guid Id { get; set; }
    public bool SimpleMode { get; set; } = false;
    public Guid? SimpleModeServiceId { get; set; }
}
