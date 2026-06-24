using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ITicketService _ticketService;
    private readonly ISettingsService _settingsService;

    public ServiceController(
        IServiceService serviceService,
        ITicketService ticketService,
        ISettingsService settingsService)
    {
        _serviceService = serviceService;
        _ticketService = ticketService;
        _settingsService = settingsService;
    }

    /// <summary>
    /// Список главных услуг (с учётом простого режима)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetServices()
    {
        var simpleMode = await _settingsService.GetSettingByNameAsync("Простой режим");

        if (simpleMode?.Value == "true")
        {
            var letter = await _settingsService.GetSettingByNameAsync("Буква для простого режима");
            return Ok(new { simpleMode = true, letter = letter?.Value });
        }

        var services = await _serviceService.GetMainServicesAsync();
        return Ok(new { simpleMode = false, services });
    }

    /// <summary>
    /// Получить услугу по id (включая дочерние)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetService(Guid id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);
        return Ok(service);
    }

    /// <summary>
    /// Создать талон для услуги
    /// </summary>
    [HttpPost("ticket")]
    public async Task<IActionResult> GetTicket([FromBody] CreateTicketRequest request)
    {
        string? facets = null;
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber) || !string.IsNullOrWhiteSpace(request.FullName))
        {
            facets = System.Text.Json.JsonSerializer.Serialize(new
            {
                phoneNumber = request.PhoneNumber,
                fullName = request.FullName
            });
        }

        var ticket = await _ticketService.CreateAsync(request.ServiceId, facets, request.Letter);
        return Ok(ticket);
    }
}

public record CreateTicketRequest(Guid ServiceId, string? Letter, string? PhoneNumber, string? FullName);