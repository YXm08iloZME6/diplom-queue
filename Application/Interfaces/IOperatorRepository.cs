using Domain.Entities;


namespace Application.Interfaces;

public interface IOperatorRepository
{
    Task<Ticket?> GetNextWaitingTicketAsync();
    Task<List<Ticket>> GetNextWaitingTicketListAsync();
    Task<Ticket?> GetCurrentTicketByWindowIdAsync(Guid windowId);
    Task UpdateTicketAsync(Ticket ticket);
    Task<Window?> GetWindowByUserIdAsync(Guid userId);
    Task SaveChangesAsync();

}
