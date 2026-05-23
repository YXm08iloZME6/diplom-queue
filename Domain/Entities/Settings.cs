using Domain.Enums;

namespace Domain.Entities;

public class Settings
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string? Value { get; set; }
    public string? Description { get; set; }
    public TypeOfSettingsValue TypeOfSettingsValue { get; set; }
}
