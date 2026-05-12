using System.Security;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class OperatorService: IOperatorService
{
    private readonly IOperatorRepository _operatorRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ITicketRepository _ticketRepository;


    public OperatorService(IOperatorRepository operatorRepository, IServiceRepository serviceRepository, ITicketRepository ticketRepository)
    {
        _operatorRepository = operatorRepository;
        _serviceRepository = serviceRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<OperatorDashboardDto> GetDashboardData(Guid userId)
    {
        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);
        if (window == null) throw new Exception("Окно не найдено");

        var serviceIds = await _serviceRepository.GetServiceTreeByIdAsync(window.ServiceId.Value);
        var currentTicket = await _operatorRepository.GetCurrentTicketByWindowIdAsync(window.Id);
        var waitingTickets = await _operatorRepository.GetNextWaitingTicketListAsync(serviceIds);

        var allServices = await _serviceRepository.GetMainServicesAsync();

        return new OperatorDashboardDto
        {
            Window = new WindowDto(window),
            CurrentTicket = currentTicket != null ? new TicketDto(currentTicket) : null,
            WaitingCount = waitingTickets.Count,
            WaitingTickets = waitingTickets.Select(t => new TicketDto(t)).ToList(),
            AllServices = allServices.Select(s => new ServiceDto(s)).ToList()
        };
    }

    public async Task<TicketDto> CallNextTicket(Guid userId)
    {
        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);

        if (window == null || window.ServiceId == null)
        {
            throw new InvalidOperationException("Окно не привязано к услуге");
        }

        var serviceIds = await _serviceRepository.GetServiceTreeByIdAsync(window.ServiceId.Value);
        var ticket = await _operatorRepository.GetNextWaitingTicketAsync(serviceIds);

        if (ticket == null)
        {
            throw new Exception("Нет ожидающих билетов.");
        }

        ticket.WindowId = window.Id;
        ticket.CalledAt = DateTime.UtcNow;
        ticket.Status = TicketStatus.Called;

        await SaveAndReturnDto(ticket);

        return new TicketDto(ticket);
    }

    public async Task<TicketDto> RecallTicket(Guid userId)
    {
        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);

        currentTicket.CalledAt = DateTime.UtcNow;

        await SaveAndReturnDto(currentTicket);

        return new TicketDto(currentTicket);
    }

    public async Task<TicketDto> CancelTicket(Guid userId)
    {
        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);

        currentTicket.Status = TicketStatus.Cancelled;
        currentTicket.CompletedAt = DateTime.UtcNow;

        await SaveAndReturnDto(currentTicket);

        return new TicketDto(currentTicket);
    }

    public async Task<TicketDto> CompleteTicket(Guid userId)
    {
        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);

        currentTicket.Status = TicketStatus.Completed;
        currentTicket.CompletedAt = DateTime.UtcNow;
        
        await SaveAndReturnDto(currentTicket);

        return new TicketDto(currentTicket);
    }

    public async Task<TicketDto> RedirectTicket(Guid userId, Guid targetServiceId, string comment)
    {
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
        currentTicket.RedirectComment = comment;

        await SaveAndReturnDto(currentTicket);

        return new TicketDto(currentTicket);
    }

    public async Task<TicketDto> StartProcessingTicket(Guid userId)
    {
        var window = await GetActiveWindowAsync(userId);
        var currentTicket = await GetCurrentTicketAsync(window.Id);

        currentTicket.Status = TicketStatus.Processing;
        
        await SaveAndReturnDto(currentTicket);

        return new TicketDto(currentTicket);
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

    private async Task SaveAndReturnDto(Ticket ticket)
    {
        await _operatorRepository.UpdateTicketAsync(ticket);
        await _operatorRepository.SaveChangesAsync();
    }
}
