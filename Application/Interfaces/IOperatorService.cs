using Application.DTOs;

namespace Application.Interfaces;

public interface IOperatorService
{
    Task<OperatorDashboardDto> GetDashboardData(Guid userId);
    Task<TicketDto> CallNextTicket(Guid userId);
    Task<TicketDto> RecallTicket(Guid userId);
    Task<TicketDto> StartProcessingTicket(Guid userId);
    Task<TicketDto> CompleteTicket(Guid userId);
    Task<TicketDto> CancelTicket(Guid userId);
    Task<TicketDto> RedirectTicket(Guid userId, Guid targetServiceId, string comment);
}
