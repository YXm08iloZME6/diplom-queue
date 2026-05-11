using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IDisplayRepository
{
    Task<List<(Window Window, Ticket? Ticket)>> GetActiveTicketsAsync();
    Task<List<Ticket>> GetWaitingTicketsAsync(int count);
}