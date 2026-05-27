using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs;

public class WindowDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public WindowStatus Status { get; set; }
    public Guid? ServiceId { get; set; }
    public string ServiceName { get; set; }
    public WindowDto() { }
    public WindowDto(Window window)
    {
        Id = window.Id;
        Title = window.Title;
        Status = window.Status;
        ServiceId = window.ServiceId;
        ServiceName = window.Service.Name;
    }
}

public class CreateWindowDto
{
    public string? Title { get; set; }
    public WindowStatus Status { get; set; }
    public Guid? ServiceId { get; set; }
}