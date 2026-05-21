using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class WindowService : IWindowService
    {
        private readonly IWindowRepository _windowRepository;

        public WindowService(IWindowRepository windowRepository)
        {
            _windowRepository = windowRepository;
        }

        public async Task<WindowDto> CreateWindowAsync(CreateWindowDto window)
        {
            var newWindow = new Window
            {
                Title = window.Title,
                Status = window.Status,
                ServiceId = window.ServiceId
            };

            await _windowRepository.CreateWindowAsync(newWindow);
            await _windowRepository.SaveChangeAsync();

            return new WindowDto
            {
                Id = newWindow.Id,
                Title = newWindow.Title,
                Status = newWindow.Status,
                ServiceId = newWindow.ServiceId
            };

        }

        public async Task<List<WindowDto>> GetAllWindows()
        {
            var windows = await _windowRepository.GetAllWindowsAsync();

            return windows.Select(w => new WindowDto
            {
                Id = w.Id,
                Title = w.Title,
                Status = w.Status,
                ServiceId = w.ServiceId
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
        
        public async Task<WindowDto> UpdateWindowStatusAsync(Guid windowId, WindowStatus status)
        {
            var window = await _windowRepository.GetWindowTitleByIdAsync(windowId);

            if (window == null)
                throw new InvalidOperationException("Окно не найдено");

            window.Status = status;
            await _windowRepository.UpdateWindowAsync(window);
            await _windowRepository.SaveChangeAsync();

            return new WindowDto
            {
                Id = window.Id,
                Title = window.Title,
                Status = window.Status,
                ServiceId = window.ServiceId
            };
        }
    }
}
