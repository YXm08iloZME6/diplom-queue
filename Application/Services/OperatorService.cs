using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;

namespace Application.Services;

public class OperatorService: IOperatorService
{
    private readonly IOperatorRepository _operatorRepository;

    public OperatorService(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<TicketDto> CallNextTicket(Guid userId)
    {
        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);

        if (window == null)
        {
            throw new Exception("Окна не существует.");
        }

        var ticket = await _operatorRepository.GetNextWaitingTicketAsync();

        if (ticket == null)
        {
            throw new Exception("Нет ожидающих билетов.");
        }

        ticket.WindowId = window.Id;
        ticket.CalledAt = DateTime.UtcNow;
        ticket.Status = TicketStatus.Called;

        await _operatorRepository.UpdateTicketAsync(ticket);
        await _operatorRepository.SaveChangesAsync();

        return new TicketDto(ticket);
    }

    public async Task<OperatorDashboardDto> GetDashboardData(Guid userId)
    {
        var window = await _operatorRepository.GetWindowByUserIdAsync(userId);
        if (window == null) throw new Exception("Окно не найдено");

        var currentTicket = await _operatorRepository.GetCurrentTicketByWindowIdAsync(window.Id);
        var waitingTickets = await _operatorRepository.GetNextWaitingTicketListAsync();

        return new OperatorDashboardDto
        {
            Window = new WindowDto(window),
            CurrentTicket = currentTicket != null ? new TicketDto(currentTicket) : null,
            WaitingCount = waitingTickets.Count,
            WaitingTickets = waitingTickets.Select(t => new TicketDto(t)).ToList()
        }; 
    }
}
