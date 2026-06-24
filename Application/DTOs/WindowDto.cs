using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs;

public class WindowDto
{
    public Guid Id { get; set; }
    public string? Number { get; set; }
    public WindowStatus Status { get; set; }
    public Guid? ServiceId { get; set; }
    public string ServiceName { get; set; }
    public WindowDto() { }
    public ServiceDto? Service { get; set; } // добавляем
    public WindowDto(Window window)
    {
        Id = window.Id;
        Number = window.Number;
        Status = window.Status;
        ServiceId = window.ServiceId;
        ServiceName = window.Service?.Name ?? "Не указано";
    }
}

public class CreateWindowDto
{
    public string? Number { get; set; }
    public WindowStatus Status { get; set; }
    public Guid? ServiceId { get; set; }
}

public class UpdateWindowDto
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Guid? ServiceId { get; set; }
}