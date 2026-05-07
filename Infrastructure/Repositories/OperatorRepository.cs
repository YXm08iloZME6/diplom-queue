using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OperatorRepository : IOperatorRepository
{
    private readonly QueueDbContext _context;

    public OperatorRepository(QueueDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket?> GetCurrentTicketByWindowIdAsync(Guid windowId)
    {
        return await _context.Tickets.FirstOrDefaultAsync(t => t.WindowId == windowId && (t.Status == TicketStatus.Called || t.Status == TicketStatus.Processing));
    }

    public async Task<Ticket?> GetNextWaitingTicketAsync()
    {
        return await _context.Tickets
            .Where(t => t.Status == TicketStatus.Waiting)
            .OrderBy(t => t.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Ticket>> GetNextWaitingTicketListAsync()
    {
        return await _context.Tickets
            .Where(t => t.Status == TicketStatus.Waiting)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Window?> GetWindowByUserIdAsync(Guid userId)
    {
        return await _context.Windows
            .Include(w => w.Operators)
            .FirstOrDefaultAsync(w => w.Operators.Any(o => o.Id == userId));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task UpdateTicketAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        return Task.CompletedTask;
    }
}
