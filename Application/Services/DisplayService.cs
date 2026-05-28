using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services;

public class DisplayService : IDisplayService
{
    private readonly IDisplayRepository _repository;
    private readonly ISettingsService _settingsService;

    public DisplayService(IDisplayRepository repository, ISettingsService settingsService)
    {
        _repository = repository;
        _settingsService = settingsService;
    }

    public async Task<DisplayDto> GetDisplayDataAsync()
    {
        var waitingCount = await _settingsService.GetSettingByNameAsync("Кол-во билетов на экране");
        var calls = await _repository.GetActiveTicketsAsync();
        var waiting = await _repository.GetWaitingTicketsAsync(Int32.Parse(waitingCount!.Value));

        return new DisplayDto
        {
            ActiveTickets = calls.Select(c => new DisplayTicketDto()
            {
                WindowId = c.window.Id,
                Title = c.window.Title,
                TicketNumber = c.ticket?.Number,
                WindowNumber = c.window.Number,
                Status = c.ticket?.Status,
            }).ToList(),
            
            WaitingTickets = waiting.Select(t => new DisplayTicketDto()
            {
                TicketNumber = t.ticket.Number,
                Status = t.ticket.Status, 
                ServiceName = t.service?.Name,
            }).ToList()
        };
    }
}