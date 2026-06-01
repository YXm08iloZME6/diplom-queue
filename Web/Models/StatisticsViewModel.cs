using Application.DTOs;
using Domain.Enums;

namespace Web.Models;

public class StatisticsViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Status { get; set; }
    public Guid? ServiceId { get; set; }

    public bool Today { get; set; }
    public bool Yesterday { get; set; }

    public List<TicketDto> Tickets { get; set; } = new();
    public List<ServiceDto> Services { get; set; } = new();
}
