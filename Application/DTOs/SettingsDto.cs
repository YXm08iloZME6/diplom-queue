using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs;

public class SettingsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public TypeOfSettingsValue TypeOfSettingsValue { get; set; }

    public SettingsDto(Settings settings)
    {
        Id = settings.Id;
        Name = settings.Name;
        Value = settings.Value;
        Description = settings.Description;
        TypeOfSettingsValue = settings.TypeOfSettingsValue;
    }
}

public class UpdateSettingsDto
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    
    public UpdateSettingsDto(Settings settings)
    {
        Id = settings.Id;
        Value = settings.Value;
    }
}