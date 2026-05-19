using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IOperatorService
{
    Task<OperatorDashboardDto> GetDashboardData(Guid userId);
    Task<TicketDto> CallNextTicket(Guid userId);
    Task<TicketDto> RecallTicket(Guid userId);
    Task<TicketDto> StartProcessingTicket(Guid userId);
    Task<TicketDto> CompleteTicket(Guid userId);
    Task<TicketDto> CancelTicket(Guid userId);
    Task<TicketDto> RedirectTicket(Guid userId, Guid targetServiceId, string comment);
    Task<WindowDto> StartShiftAsync(Guid userId);//добавил дэнчик
    Task<WindowDto> EndShiftAsync(Guid userId);//добавил дэнчик
}
