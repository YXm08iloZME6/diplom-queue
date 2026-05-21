using Application.Interfaces;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QueueController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ITicketService _ticketService;
    private readonly IWindowService _windowService;

    public QueueController(
        IServiceService serviceService,
        ITicketService ticketService,
        IWindowService windowService)
    {
        _serviceService = serviceService;
        _ticketService = ticketService;
        _windowService = windowService;
    }

    /// <summary>
    /// Список всех услуг (очередей)
    /// </summary>
    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var services = await _serviceService.GetMainServicesAsync();
        return Ok(services);
    }

    /// <summary>
    /// Список всех окон
    /// </summary>
    [HttpGet("windows")]
    public async Task<IActionResult> GetWindows()
    {
        var windows = await _windowService.GetAllWindows();
        return Ok(windows);
    }

    /// <summary>
    /// Все талоны
    /// </summary>
    [HttpGet("tickets")]
    public async Task<IActionResult> GetTickets()
    {
        var tickets = await _ticketService.GetAllAsync();
        return Ok(tickets);
    }

    /// <summary>
    /// Общая сводка: количество талонов по каждой услуге
    /// </summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var tickets = await _ticketService.GetAllAsync();
        var services = await _serviceService.GetMainServicesAsync();

        var summary = services.Select(s => new
        {
            s.Id,
            s.Name,
            s.Letter,
            Total = tickets.Count(t => t.ServiceId == s.Id),
            Waiting = tickets.Count(t => t.ServiceId == s.Id && t.Status == "Waiting"),
            Processing = tickets.Count(t => t.ServiceId == s.Id && t.Status == "Processing"),
            Completed = tickets.Count(t => t.ServiceId == s.Id && t.Status == "Completed"),
        });

        return Ok(summary);
    }
}