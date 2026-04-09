using Domain.Entities;

namespace Application.Interfaces;

public interface ITicketRepository
{
    Task<Ticket> AddAsync(Ticket ticket);
    Task<string> GenNumberAsync(Guid serviceId);
}