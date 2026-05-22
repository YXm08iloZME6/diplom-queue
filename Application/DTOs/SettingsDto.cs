namespace Application.DTOs;

public class SettingsDto
{
    public Guid Id { get; set; }
    public bool SimpleMode { get; set; }
    public Guid? SimpleModeServiceId { get; set; }
}