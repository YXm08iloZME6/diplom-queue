using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IWindowRepository
{
    Task<List<Window>> GetAllWindowsAsync();
    Task<Window> GetWindowTitleByIdAsync(Guid windowId);
    Task CreateWindowAsync(Window window);
    Task SaveChangeAsync();
}
