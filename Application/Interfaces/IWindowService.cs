using Application.DTOs;

namespace Application.Interfaces;

public interface IWindowService
{
    Task<List<WindowDto>> GetAllWindows();
    Task<WindowDto> GetWindowById(Guid windowId);
}
