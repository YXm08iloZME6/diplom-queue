using Domain.Enums;
namespace Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? CalledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? RedirectComment { get; set; }


    public TimeSpan? EstimationTime { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.Waiting;
    public string Number { get; set; } = "A-999";
    public string? Facets { get; set; }
    
    public Guid? ClientId { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? WindowId { get; set; }
}