using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IQueueNotifier
{
    Task NotifyNewTicketAsync(TicketDto ticket, CancellationToken ct = default);
    Task NotifyUpdateTicketAsync(TicketDto ticket, CancellationToken ct = default);
    Task NotifyCallTicketAsync(TicketDto ticket, CancellationToken ct = default);
    Task NotifyRecallTicketAsync(TicketDto ticket, CancellationToken ct = default);
}