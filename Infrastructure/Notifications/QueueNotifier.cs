
using Application.DTOs;
using Application.Interfaces.Services;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notifications;

public class QueueNotifier : IQueueNotifier
{
    private readonly IHubContext<QueueHub> _hubContext;

    public QueueNotifier(IHubContext<QueueHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyNewTicketAsync(TicketDto ticket, CancellationToken ct = default)
    {
        await _hubContext.Clients.All.SendAsync("NewTicket", ticket, ct);
    }

    public async Task NotifyUpdateTicketAsync(TicketDto ticket, CancellationToken ct = default)
    {
        await _hubContext.Clients.All.SendAsync("UpdateTicket", ticket, ct);
    }

    public async Task NotifyCallTicketAsync(TicketDto ticket, CancellationToken ct = default)
    {
        await _hubContext.Clients.All.SendAsync("CallTicket", ticket, ct);
    }

    public async Task NotifyRecallTicketAsync(TicketDto ticket, CancellationToken ct = default)
    {
        await _hubContext.Clients.All.SendAsync("RecallTicket", ticket, ct);
    }
}
