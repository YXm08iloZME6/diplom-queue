using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IQueueNotifier
{
    Task NotifyNewTicketAsync(TicketDto ticket, CancellationToken ct = default);
}
