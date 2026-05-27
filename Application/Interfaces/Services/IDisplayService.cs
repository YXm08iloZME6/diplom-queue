using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IDisplayService
{
    Task<DisplayDto> GetDisplayDataAsync();
}