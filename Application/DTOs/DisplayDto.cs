namespace Application.DTOs;

public class DisplayDto
{
    public List<DisplayTicketDto> ActiveTickets { get; set; } = new();
    public List<TicketDto> WaitingTickets { get; set; } = new();
}

public class DisplayTicketDto
{
    public Guid WindowId { get; set; }
    public string Title { get; set; } = "";
    public string? TicketNumber { get; set; }
    public string WindowNumber { get; set; } = "";
}