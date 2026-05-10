using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class WindowService : IWindowService
    {
        private readonly IWindowRepository _windowRepository;

        public WindowService(IWindowRepository windowRepository)
        {
            _windowRepository = windowRepository;
        }

        public async Task<List<WindowDto>> GetAllWindows()
        {
            var windows = await _windowRepository.GetAllWindowsAsync();

            return windows.Select(w => new WindowDto
            {
                Id = w.Id,
                Title = w.Title,
                Status = w.Status
            }).ToList();
        }

        public async Task<WindowDto> GetWindowById(Guid windowId)
        {
            var window = await _windowRepository.GetWindowTitleByIdAsync(windowId);

            if (window == null)
            {
                return null;
            }

            return new WindowDto
            {
                Id = window.Id,
                Title = window.Title,
                Status = window.Status
            };
        }
    }
}
