using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IWindowService
{
    Task<List<WindowDto>> GetAllWindows();
    Task<WindowDto> GetWindowById(Guid windowId);
    Task<WindowDto> CreateWindowAsync(CreateWindowDto window);
}
