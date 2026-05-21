using Application.DTOs;

namespace Application.Interfaces.Services;

public interface ITicketService
{
    Task<TicketDto> CreateAsync(Guid serviceId, string? info, string? letter);
    Task<TicketDto?> GetByIdAsync(Guid id);
    Task<List<TicketDto>> GetAllAsync();
    Task<TicketDto> UpdateAsync(Guid id, UpdateTicketDto dto);
    Task<bool> DeleteAsync(Guid id);

    Task<TicketDto> CallAsync(Guid ticketId, Guid windowId);
    Task<TicketDto> RecallAsync(Guid ticketId);
    Task<TicketDto> StartProcessingAsync(Guid ticketId);
    Task<TicketDto> CompleteAsync(Guid ticketId);
    Task<TicketDto> CancelAsync(Guid ticketId);
    Task<TicketDto> RedirectAsync(Guid ticketId, Guid newServiceId, string comment);
}