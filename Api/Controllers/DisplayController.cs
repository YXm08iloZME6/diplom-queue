using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DisplayController : ControllerBase
{
    private readonly IDisplayService _displayService;

    public DisplayController(IDisplayService displayService)
    {
        _displayService = displayService;
    }

    /// <summary>
    /// Все данные для экрана — активные талоны и очередь ожидания
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetDisplayData()
    {
        var data = await _displayService.GetDisplayDataAsync();
        return Ok(data);
    }

    /// <summary>
    /// Только активные талоны (кто сейчас обслуживается)
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveTickets()
    {
        var data = await _displayService.GetDisplayDataAsync();
        return Ok(data.ActiveTickets);
    }

    /// <summary>
    /// Только очередь ожидания
    /// </summary>
    [HttpGet("waiting")]
    public async Task<IActionResult> GetWaitingTickets()
    {
        var data = await _displayService.GetDisplayDataAsync();
        return Ok(data.WaitingTickets);
    }
}