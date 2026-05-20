using Application.DTOs;

namespace Web.Models;

public class StatisticsViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<TicketDto> Tickets { get; set; } = new();
}
