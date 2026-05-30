using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id);
    Task<List<Ticket>> GetAllAsync();
    Task<List<Ticket>> GetAllActiveAsync();
    Task<List<Ticket>> GetFilteredTicketsAsync(DateTime start, DateTime end, string? status = null, Guid? serviceId = null);

    Task AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(Ticket ticket);
    Task<int> GetTicketCountAsync(string? letter);
}