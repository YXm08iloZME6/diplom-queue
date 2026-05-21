namespace Application.DTOs;

public class OperatorDashboardDto
{
    public WindowDto Window { get; set; }

    public TicketDto? CurrentTicket { get; set; }

    public int WaitingCount { get; set; }
    public List<TicketDto> WaitingTickets { get; set; } = new();
    public List<ServiceDto> AllServices { get; set; } = new();
}

