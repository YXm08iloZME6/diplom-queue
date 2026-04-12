using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repository; 
    
    public TicketService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketDto> CreateTicketAsync(Guid serviceId, string info, string letter)
    {
        var count = await _repository.GetTicketCountAsync(serviceId);

        Ticket ticket = new Ticket
        {
            ServiceId = serviceId,
            CreatedAt = DateTime.UtcNow,
            Number = $"{letter}-{(count + 1):D3}",
            Facets = info
        };

        await _repository.AddAsync(ticket);

        return new TicketDto(ticket);
    }
}