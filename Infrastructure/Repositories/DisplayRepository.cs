using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DisplayRepository : IDisplayRepository
{
    private readonly QueueDbContext _dbContext;
    
    public DisplayRepository(QueueDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<List<(Window window, Ticket? ticket)>> GetActiveTicketsAsync()
    {
        var windows = await _dbContext.Windows!.ToListAsync();
        var tickets = await _dbContext.Tickets!
            .Where(t => t.WindowId.HasValue
                        && (t.Status == TicketStatus.Called || t.Status == TicketStatus.Processing))
            .ToListAsync();

        return windows
            .Select(w => (Window: w, Ticket: tickets.FirstOrDefault(t => t.WindowId == w.Id)))
            .ToList();
    }

    public async Task<List<(Ticket ticket, Service? service)>> GetWaitingTicketsAsync(int count)
    {
        var services = await _dbContext.Services!.ToListAsync();
        var tickets = await _dbContext.Tickets!
            .Where(t => t.Status == TicketStatus.Waiting)
            .OrderBy(t => t.CreatedAt)
            .Take(count)
            .ToListAsync(); 
        
        return tickets.Select(t => (Ticket: t, Service: services.FirstOrDefault(s => s.Id == t.ServiceId)))
            .ToList();
    }
}