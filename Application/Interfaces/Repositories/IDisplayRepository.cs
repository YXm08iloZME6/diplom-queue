using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IDisplayRepository
{
    Task<List<(Window window, Ticket? ticket)>> GetActiveTicketsAsync();
    Task<List<(Ticket ticket, Service? service)>> GetWaitingTicketsAsync(int count);
}