using Domain.Entities;

namespace Application.Interfaces;

public interface IWindowRepository
{
    Task<List<Window>> GetAllWindowsAsync();
    Task<Window> GetWindowTitleByIdAsync(Guid windowId);
}
