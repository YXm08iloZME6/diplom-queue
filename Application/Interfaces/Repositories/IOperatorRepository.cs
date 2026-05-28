using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOperatorRepository
{
    Task<Ticket?> GetNextWaitingTicketAsync(List<Guid> serviceIds);
    Task<List<Ticket?>> GetWaitingTickets();
    Task<List<Ticket>> GetNextWaitingTicketListAsync(List<Guid> serviceIds);
    Task<Ticket?> GetCurrentTicketByWindowIdAsync(Guid windowId);
    Task<Ticket?> GetNextWaitingTicketWithoutServiceId();
    Task<Ticket?> GetCurrentTicketWithoutWindowId();
    Task UpdateTicketAsync(Ticket ticket);
    Task<Window?> GetWindowByUserIdAsync(Guid userId);
    Task UpdateWindowAsync(Window window);//добавил дэнчик

}
