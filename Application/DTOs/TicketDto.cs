using Domain.Entities;

namespace Application.DTOs;

public class TicketDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } 
    public string Number { get; set; }

    public TicketDto(Ticket ticket)
    {
       Id = ticket.Id;
       CreatedAt = ticket.CreatedAt;
       Status = ticket.Status.ToString();
       Number = ticket.Number;
    }
}