using System.Security.Authentication.ExtendedProtection;
using Domain.Entities;

namespace Application.DTOs;

public class TicketDto
{
    public Guid Id { get; set; }

    public string Number { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? CalledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public Guid? ServiceId { get; set; }
    public Guid? WindowId { get; set; }

    public string? RedirectComment { get; set; }

    public TicketDto(Ticket ticket)
    {
        Id = ticket.Id;
        CreatedAt = ticket.CreatedAt;
        CompletedAt = ticket.CompletedAt;
        Status = ticket.Status.ToString();
        Number = ticket.Number;
    }

    public TicketDto(Ticket ticket, int utcOffset)
    {
        Id = ticket.Id;
        Number = ticket.Number;
        Status = ticket.Status.ToString();

        CreatedAt = ticket.CreatedAt?.AddHours(utcOffset);

        StartedAt = ticket.StartedAt?.AddHours(utcOffset);

        CompletedAt = ticket.CompletedAt?.AddHours(utcOffset);
    }
}

public class UpdateTicketDto
{
    public Guid? ServiceId { get; set; }
    public Guid? WindowId { get; set; }
    public string? Number { get; set; }
    public string? Facets { get; set; }
    public string? RedirectComment { get; set; }
}


public class RedirectTicketDto
{
    public Guid Id { get; set; }
    public Guid NewServiceId { get; set; }
    public string RedirectComment { get; set; }
}