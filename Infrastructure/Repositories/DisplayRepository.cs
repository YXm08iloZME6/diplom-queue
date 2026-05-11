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


    public async Task<List<(Window Window, Ticket? Ticket)>> GetActiveTicketsAsync()
    {
        var windows = await _dbContext.Windows!.ToListAsync();
        var windowIds = windows.Select(w => w.Id).ToList();

        var tickets = await _dbContext.Tickets!
            .Where(t => t.WindowId.HasValue
                        && windowIds.Contains(t.WindowId.Value)
                        && (t.Status == TicketStatus.Called || t.Status == TicketStatus.Processing))
            .ToListAsync();

        return windows
            .Select(w => (Window: w, Ticket: tickets.FirstOrDefault(t => t.WindowId == w.Id)))
            .ToList();
    }

    public async Task<List<Ticket>> GetWaitingTicketsAsync(int count)
    {
        return await _dbContext.Tickets!
            .Where(t => t.Status == TicketStatus.Waiting)
            .OrderBy(t => t.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}