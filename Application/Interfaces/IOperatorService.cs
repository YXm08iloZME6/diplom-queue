using Application.DTOs;

namespace Application.Interfaces;

public interface IOperatorService
{
    Task<OperatorDashboardDto> GetDashboardData(Guid userId);
    Task<TicketDto> CallNextTicket(Guid userId);
}
