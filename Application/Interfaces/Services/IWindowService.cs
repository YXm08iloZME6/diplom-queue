using Application.DTOs;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IWindowService
{
    Task<List<WindowDto>> GetAllWindows();
    Task<WindowDto> GetWindowById(Guid windowId);
    Task<WindowDto> CreateWindowAsync(CreateWindowDto window);
    Task UpdateWindowAsync(UpdateWindowDto window);
    Task DeleteWindowAsync(Guid id);
    Task<WindowDto> UpdateWindowStatusAsync(Guid windowId, WindowStatus status); //добавил дэнчик
}
