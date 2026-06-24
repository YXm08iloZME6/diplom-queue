using Application.Interfaces.Repositories;
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

    public async Task CreateWindowAsync(Window window)
    {
        
        await _context.Windows.AddAsync(window);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Window>> GetAllWindowsAsync()
    {
        return await _context.Windows
            .Include(w => w.Service)
            .ToListAsync();
    }

    public async Task<Window> GetWindowByIdAsync(Guid windowId)
    {
        return await _context.Windows.FirstOrDefaultAsync(w => w.Id == windowId);
    }
    
    public async Task UpdateWindowAsync(Window window) //дэнчик
    {
        _context.Windows.Update(window);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteWindowAsync(Window window)
    {
        _context.Windows.Remove(window);
        await _context.SaveChangesAsync();
    }
}
