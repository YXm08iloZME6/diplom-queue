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
        return ticket;
    }

    public async Task<string> GenNumberAsync(Guid serviceId)
    {
        var service = await _dbContext.Set<Service>().FindAsync(serviceId);
        var count = await _dbContext.Set<Ticket>().Where(t => t.ServiceId == serviceId).CountAsync();
        
        return $"{service?.Letter}-{(count + 1):D3}";
    }
}