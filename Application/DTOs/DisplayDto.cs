using Domain.Enums;

namespace Application.DTOs;

public class DisplayDto
{
    public List<DisplayTicketDto> ActiveTickets { get; set; } = new();
    public List<DisplayTicketDto> WaitingTickets { get; set; } = new();
}

public class DisplayTicketDto
{
    public Guid? WindowId { get; set; }
    public string? TicketNumber { get; set; }
    public string? WindowNumber { get; set; }
    public TicketStatus? Status { get; set; }
    public string? ServiceName { get; set; } = "";
}