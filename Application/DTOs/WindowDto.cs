using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs;

public class WindowDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public WindowStatus Status { get; set; }
    public Guid? ServiceId { get; set; }
    public WindowDto(Window window)
    {
        Id = window.Id;
        Title = window.Title;
        Status = window.Status;
        ServiceId = window.ServiceId;
    }


    public class UpdateWindowDto
    {
        public string Title { get; set; }
        public WindowStatus Status { get; set; }
        public Guid? ServiceId { get; set; }
    }
}
