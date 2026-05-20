using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "operator")]
public class OperatorController : ControllerBase
{
    private readonly IOperatorService _operatorService;

    public OperatorController(IOperatorService operatorService)
    {
        _operatorService = operatorService;
    }
    
    /// <summary>
    /// Начать смену — открыть окно
    /// </summary>
    [HttpPost("start-shift")]
    public async Task<IActionResult> StartShift()
    {
        var userId = GetUserId();
        var window = await _operatorService.StartShiftAsync(userId);
        return Ok(window);
    }

    /// <summary>
    /// Завершить смену — закрыть окно
    /// </summary>
    [HttpPost("end-shift")]
    public async Task<IActionResult> EndShift()
    {
        var userId = GetUserId();
        var window = await _operatorService.EndShiftAsync(userId);
        return Ok(window);
    }

    /// <summary>
    /// Текущее состояние панели оператора
    /// </summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var userId = GetUserId();
        var data = await _operatorService.GetDashboardData(userId);
        return Ok(data);
    }
    
    /// <summary>
    /// Вызвать следующий талон из очереди
    /// </summary>
    [HttpPost("call-next")]
    public async Task<IActionResult> CallNext()
    {
        var userId = GetUserId();
        var ticket = await _operatorService.CallNextTicket(userId);
        return Ok(ticket);
    }

    /// <summary>
    /// Перевызвать текущий талон (обновить на экране)
    /// </summary>
    [HttpPost("recall")]
    public async Task<IActionResult> Recall()
    {
        var userId = GetUserId();
        var ticket = await _operatorService.RecallTicket(userId);
        return Ok(ticket);
    }

    /// <summary>
    /// Начать обслуживание — клиент подошёл к окну
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> StartProcessing()
    {
        var userId = GetUserId();
        var ticket = await _operatorService.StartProcessingTicket(userId);
        return Ok(ticket);
    }

    /// <summary>
    /// Завершить обслуживание — талон уходит в архив
    /// </summary>
    [HttpPost("complete")]
    public async Task<IActionResult> Complete()
    {
        var userId = GetUserId();
        var ticket = await _operatorService.CompleteTicket(userId);
        return Ok(ticket);
    }

    /// <summary>
    /// Отменить талон
    /// </summary>
    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel()
    {
        var userId = GetUserId();
        var ticket = await _operatorService.CancelTicket(userId);
        return Ok(ticket);
    }

    /// <summary>
    /// Перенаправить талон в другую очередь
    /// </summary>
    [HttpPost("redirect")]
    public async Task<IActionResult> Redirect([FromBody] RedirectRequest request)
    {
        var userId = GetUserId();
        var ticket = await _operatorService.RedirectTicket(userId, request.ServiceId, request.Comment);
        return Ok(ticket);
    }
    
    private Guid GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!);
    }
}

public record RedirectRequest(Guid ServiceId, string Comment);