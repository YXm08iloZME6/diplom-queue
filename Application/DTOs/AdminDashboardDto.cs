namespace Application.DTOs;

public class AdminDashboardDto
{
    public int TicketsToday { get; set; }
    public int CompletedToday { get; set; }
    public int WaitingNow { get; set; }
    public int ActiveOperators { get; set; }

    public List<ServiceStatDto> ServiceStats { get; set; } = new();
    public List<TicketDto> LastTickets { get; set; } = new();
    public List<WindowDto> Windows { get; set; } = new(); 
}