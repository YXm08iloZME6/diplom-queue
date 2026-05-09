using Domain.Entities;


namespace Application.Interfaces;

public interface IOperatorRepository
{
    Task<Ticket?> GetNextWaitingTicketAsync(List<Guid> serviceIds);
    Task<List<Ticket>> GetNextWaitingTicketListAsync(List<Guid> serviceIds);
    Task<Ticket?> GetCurrentTicketByWindowIdAsync(Guid windowId);
    Task UpdateTicketAsync(Ticket ticket);
    Task<Window?> GetWindowByUserIdAsync(Guid userId);
    Task SaveChangesAsync();

}
