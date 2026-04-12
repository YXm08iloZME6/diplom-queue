using Application.DTOs;

namespace Application.Interfaces;

public interface ITicketService
{
    Task<TicketDto> CreateTicketAsync(Guid serviceId, string info, string letter);
}