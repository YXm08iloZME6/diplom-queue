using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly QueueDbContext _dbContext;

    public TicketRepository(QueueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Ticket?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Tickets!.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Ticket>> GetAllAsync()
    {
        return await _dbContext.Tickets!
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Ticket ticket)
    {
        await _dbContext.Tickets!.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _dbContext.Tickets!.Update(ticket);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ticket ticket)
    {
        _dbContext.Tickets!.Remove(ticket);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetTicketCountAsync(string letter)
    {
        return await _dbContext.Set<Ticket>().Where(t => t.Number.StartsWith(letter)).CountAsync();
    }

    public async Task<List<Ticket>> GetAllActiveAsync()
    {
        var targetStatus = new List<TicketStatus> { TicketStatus.Waiting, TicketStatus.Processing, TicketStatus.Called };

        return await _dbContext.Tickets.Where(t => targetStatus.Contains(t.Status)).ToListAsync();
    }

    public Task SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public async Task<List<Ticket>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _dbContext.Tickets
            .Where(t => t.CompletedAt >= start && t.CompletedAt <= end)
            .ToListAsync();
    }
}