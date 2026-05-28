using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IWindowRepository
{
    Task<List<Window>> GetAllWindowsAsync();
    Task<Window> GetWindowByIdAsync(Guid windowId);
    Task CreateWindowAsync(Window window);
    Task UpdateWindowAsync(Window window);//добавил дэнчик
    Task DeleteWindowAsync(Window window);
}
