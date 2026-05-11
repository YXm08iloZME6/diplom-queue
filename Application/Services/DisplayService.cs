using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services;

public class DisplayService : IDisplayService
{
    private readonly IDisplayRepository _repository;

    public DisplayService(IDisplayRepository repository)
    {
        _repository = repository;
    }

    public async Task<DisplayDto> GetDisplayDataAsync(int waitingCount = 5)
    {
        var calls = await _repository.GetActiveTicketsAsync();
        var waiting = await _repository.GetWaitingTicketsAsync(waitingCount);

        return new DisplayDto
        {
            ActiveTickets = calls.Select(c => new DisplayTicketDto()
            {
                WindowId = c.Window.Id,
                Title = c.Window.Title ?? "",
                TicketNumber = c.Ticket?.Number,
                WindowNumber = c.Window.Number,
            }).ToList(),
            
            WaitingTickets = waiting.Select(t => new TicketDto(t)).ToList()
        };
    }
}