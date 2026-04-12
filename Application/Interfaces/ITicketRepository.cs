using Domain.Entities;

namespace Application.Interfaces;

public interface ITicketRepository
{
    Task<Ticket> AddAsync(Ticket ticket);
    Task<int> GetTicketCountAsync(Guid serviceId);
}