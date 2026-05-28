using System.Net.Sockets;
using System.Security;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class OperatorService : IOperatorService
{
    private readonly IOperatorRepository _operatorRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ISettingsRepository _settingsRepository;
    private readonly IQueueNotifier _queueNotifier;
    private readonly IUserRepository _userRepository;

    public OperatorService(IOperatorRepository operatorRepository, IServiceRepository serviceRepository, 
        ITicketRepository ticketRepository, ISettingsRepository settingsRepository, IQueueNotifier queueNotifier, IUserRepository userRepository)
    {
        _operatorRepository = operatorRepository;
        _serviceRepository = serviceRepository;
        _ticketRepository = ticketRepository;
        _settingsRepository = settingsRepository;
        _queueNotifier = queueNotifier;
        _userRepository = userRepository;
    }

    public async Task<OperatorDashboardDto> GetDashboardData(Guid userId)
    {
        var settingSimpleMode = await _settingsRepository.GetSettingByNameAsync("Простой режим");

        if (settingSimpleMode.Value == "true")
        {
            var simpleCurrentTicket = await _operatorRepository.GetCurrentTicketWithoutWindowId();
            var simpleWaitingTickets = await _operatorRepository.GetWaitingTickets();

            return new OperatorDashboardDto
            {
                CurrentTicket = simpleCurrentTicket != null ? new TicketDto(simpleCurrentTicket) : null,
                WaitingCount = simpleWaitingTickets.Count,
                WaitingTickets = simpleWaitingTickets.Select(t => new TicketDto(t)).ToList(),
            };
        }

        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);
        if (window == null) throw new Exception("Окно не найдено");

        var serviceIds = await _serviceRepository.GetServiceTreeByIdAsync(window.ServiceId.Value);
        var currentTicket = await _operatorRepository.GetCurrentTicketByWindowIdAsync(window.Id);
        var waitingTickets = await _operatorRepository.GetNextWaitingTicketListAsync(serviceIds);

        var allServices = await _serviceRepository.GetMainServicesAsync();

        var filteredServices = allServices.Where(s => s.Id != window.ServiceId).ToList();

        return new OperatorDashboardDto
        {
            Window = new WindowDto(window),
            CurrentTicket = currentTicket != null ? new TicketDto(currentTicket) : null,
            WaitingCount = waitingTickets.Count,
            WaitingTickets = waitingTickets.Select(t => new TicketDto(t)).ToList(),
            AllServices = filteredServices.Select(s => new ServiceDto(s)).ToList()
        };
    }

    public async Task<TicketDto> CallNextTicket(Guid userId)
    {
        var settingSimpleMode = await _settingsRepository.GetSettingByNameAsync("Простой режим");
        
        if (settingSimpleMode.Value == "true")
        {
            var simpleTicket = await _operatorRepository.GetNextWaitingTicketWithoutServiceId();

            if (simpleTicket == null) throw new Exception("Нет ожидающих билетов");

            simpleTicket!.Status = TicketStatus.Called;
            simpleTicket.CalledAt = DateTime.UtcNow;

            await _operatorRepository.UpdateTicketAsync(simpleTicket);
            await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(simpleTicket));
            return new TicketDto(simpleTicket);
        }

        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);
        var user = await _userRepository.GetByIdAsync(userId);

        if (window == null || window.ServiceId == null)
        {
            throw new InvalidOperationException("Окно не привязано к услуге");
        }

        var serviceIds = await _serviceRepository.GetServiceTreeByIdAsync(window.ServiceId.Value);
        var ticket = await _operatorRepository.GetNextWaitingTicketAsync(serviceIds);
        var offset = await GetUtcOffset();

        if (ticket == null)
        {
            throw new Exception("Нет ожидающих билетов.");
        }

        ticket.WindowId = window.Id;
        ticket.CalledAt = DateTime.UtcNow;
        ticket.Status = TicketStatus.Called;
        user.Status = UserStatus.Busy;
        window.Status = WindowStatus.Busy;

        await _operatorRepository.UpdateTicketAsync(ticket);
        await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(ticket));

        return new TicketDto(ticket, offset);
    }

    public async Task<TicketDto> RecallTicket(Guid userId)
    {
        var settingSimpleMode = await _settingsRepository.GetSettingByNameAsync("Простой режим");

        if (settingSimpleMode.Value == "true")
        {
            var simpleTicket = await _operatorRepository.GetCurrentTicketWithoutWindowId();

            if (simpleTicket == null) throw new Exception("Нет текущего билета");

            simpleTicket.CalledAt = DateTime.UtcNow;

            await _operatorRepository.UpdateTicketAsync(simpleTicket);
            await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(simpleTicket));

            return new TicketDto(simpleTicket);
        }

        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);
        var offset = await GetUtcOffset();

        currentTicket.CalledAt = DateTime.UtcNow;

        await _operatorRepository.UpdateTicketAsync(currentTicket);
        await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(currentTicket));

        return new TicketDto(currentTicket, offset);
    }

    public async Task<TicketDto> CancelTicket(Guid userId)
    {
        var settingSimpleMode = await _settingsRepository.GetSettingByNameAsync("Простой режим");

        if (settingSimpleMode.Value == "true")
        {
            var simpleTicket = await _operatorRepository.GetCurrentTicketWithoutWindowId();

            if (simpleTicket == null)
                throw new Exception("Нет текущего билета");

            simpleTicket.Status = TicketStatus.Cancelled;
            simpleTicket.CompletedAt = DateTime.UtcNow;

            await _operatorRepository.UpdateTicketAsync(simpleTicket);
            await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(simpleTicket));

            return new TicketDto(simpleTicket);
        }

        var user = await _userRepository.GetByIdAsync(userId);
        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);
        var offset = await GetUtcOffset();

        currentTicket.Status = TicketStatus.Cancelled;
        currentTicket.CompletedAt = DateTime.UtcNow;
        user.Status = UserStatus.Waiting;
        window.Status = WindowStatus.Open;

        await _operatorRepository.UpdateTicketAsync(currentTicket);
        await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(currentTicket));

        return new TicketDto(currentTicket, offset);
    }

    public async Task<TicketDto> CompleteTicket(Guid userId)
    {
        var settingSimpleMode = await _settingsRepository.GetSettingByNameAsync("Простой режим");

        if (settingSimpleMode.Value == "true")
        {
            var simpleTicket = await _operatorRepository.GetCurrentTicketWithoutWindowId();

            if (simpleTicket == null)
                throw new Exception("Нет текущего билета");

            simpleTicket.Status = TicketStatus.Completed;
            simpleTicket.CompletedAt = DateTime.UtcNow;

            await _operatorRepository.UpdateTicketAsync(simpleTicket);
            await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(simpleTicket));

            return new TicketDto(simpleTicket);
        }

        var user = await _userRepository.GetByIdAsync(userId);
        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);
        var offset = await GetUtcOffset();

        currentTicket.Status = TicketStatus.Completed;
        currentTicket.CompletedAt = DateTime.UtcNow;
        user.Status = UserStatus.Waiting;
        window.Status = WindowStatus.Open;

        await _operatorRepository.UpdateTicketAsync(currentTicket);
        await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(currentTicket));

        return new TicketDto(currentTicket, offset);
    }

    public async Task<TicketDto> RedirectTicket(Guid userId, Guid targetServiceId, string comment)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);

        var targetService = await _serviceRepository.GetServiceByIdAsync(targetServiceId);
        Console.WriteLine(targetServiceId);
        if (targetService == null) throw new Exception("Сервис для перенаправления не найден");

        if (!string.IsNullOrEmpty(targetService.Letter))
        {
            var countTargetService = await _ticketRepository.GetTicketCountAsync(targetService.Letter);
            currentTicket.Number = $"{targetService.Letter}-{(countTargetService + 1):D3}";
        }

        currentTicket.ServiceId = targetServiceId;
        currentTicket.Status = TicketStatus.Waiting;
        currentTicket.WindowId = null;
        currentTicket.CalledAt = null;
        currentTicket.RedirectComment = $"Перенаправлен из окна номер {window.Title} с комментарием:" + comment;
        user.Status = UserStatus.Waiting;
        window.Status = WindowStatus.Open;

        await _operatorRepository.UpdateTicketAsync(currentTicket);
        await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(currentTicket));

        return new TicketDto(currentTicket);
    }

    public async Task<TicketDto> StartProcessingTicket(Guid userId)
    {
        var settingSimpleMode = await _settingsRepository.GetSettingByNameAsync("Простой режим");

        if (settingSimpleMode.Value == "true")
        {
            var simpleTicket = await _operatorRepository.GetCurrentTicketWithoutWindowId();

            if (simpleTicket == null) throw new Exception("Нет текущего билета");

            simpleTicket.Status = TicketStatus.Processing;
            simpleTicket.StartedAt = DateTime.UtcNow;

            await _operatorRepository.UpdateTicketAsync(simpleTicket);
            await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(simpleTicket));

            return new TicketDto(simpleTicket);
        }

        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);
        var offset = await GetUtcOffset();

        currentTicket.Status = TicketStatus.Processing;
        currentTicket.StartedAt = DateTime.UtcNow;

        await _operatorRepository.UpdateTicketAsync(currentTicket);
        await _queueNotifier.NotifyUpdateTicketAsync(new TicketDto(currentTicket));

        return new TicketDto(currentTicket, offset);
    }


    private async Task<Window> GetActiveWindowAsync(Guid userId)
    {
        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);
        if (window == null) throw new Exception("Окно не найдено");
        return window;
    }

    private async Task<Ticket> GetCurrentTicketAsync(Guid windowId)
    {
        var currentTicket = await _operatorRepository.GetCurrentTicketByWindowIdAsync(windowId);
        if (currentTicket == null) throw new Exception("Нет текущего билета");
        return currentTicket;
    }

    private async Task<int> GetUtcOffset()
    {
        var setting = await _settingsRepository.GetSettingByNameAsync("Часовой пояс");
        if (setting == null || !int.TryParse(setting.Value, out int offset))
        {
            return 0;
        }

        return offset;
    }

    public async Task<WindowDto> StartShiftAsync(Guid userId)
    {
        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);
        if (window == null) throw new Exception("Окно не найдено");

        window.Status = WindowStatus.Open;
        await _operatorRepository.UpdateWindowAsync(window);

        return new WindowDto(window);
    }

    public async Task<WindowDto> EndShiftAsync(Guid userId)
    {
        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);
        if (window == null) throw new Exception("Окно не найдено");

        window.Status = WindowStatus.Offline;
        await _operatorRepository.UpdateWindowAsync(window);

        return new WindowDto(window);
    }
}
