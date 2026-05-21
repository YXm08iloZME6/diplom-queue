using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id);
    Task<List<Ticket>> GetAllAsync();
    Task<List<Ticket>> GetAllActiveAsync();
    Task<List<Ticket>> GetByDateRangeAsync(DateTime start, DateTime end);
    
    Task AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(Ticket ticket);
    Task SaveChangesAsync();
    Task<int> GetTicketCountAsync(string? letter);
}