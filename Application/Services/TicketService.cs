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

    public async Task<TicketDto> CreateTicketAsync(Guid serviceId)
    {
        Ticket ticket = new Ticket
        {
            ServiceId = serviceId,
            CreatedAt = DateTime.UtcNow,
            Number = await _repository.GenNumberAsync(serviceId),
        };
        
        await _repository.AddAsync(ticket);
        
        return new TicketDto(ticket);
    }
}