using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WindowRepository : IWindowRepository
{
    private readonly QueueDbContext _context;

    public WindowRepository(QueueDbContext context)
    {
        _context = context;
    }

    public async Task<List<Window>> GetAllWindowsAsync()
    {
        return await _context.Windows.ToListAsync();
    }

    public async Task<Window> GetWindowTitleByIdAsync(Guid windowId)
    {
        return await _context.Windows.FirstOrDefaultAsync(w => w.Id == windowId);
    }
}
