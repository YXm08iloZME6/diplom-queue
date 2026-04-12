using Application.Interfaces;
using Domain.Entities;
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
    
    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        await _dbContext.Set<Ticket>().AddAsync(ticket);
        await _dbContext.SaveChangesAsync();
        return ticket;
    }

    public async Task<int> GetTicketCountAsync(Guid serviceId)
    {
        return await _dbContext.Set<Ticket>().Where(t => t.ServiceId == serviceId).CountAsync();
    }
}