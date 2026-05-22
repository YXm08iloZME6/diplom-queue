using System.Diagnostics.Metrics;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ServiceController : Controller
{
    private readonly IServiceService _serviceService;
    private readonly ITicketService _ticketService;
    private readonly ISettingsService _settingsService;

    public ServiceController(IServiceService serviceService, ITicketService ticketService, ISettingsService settingsService)
    {
        _serviceService = serviceService;
        _ticketService = ticketService;
        _settingsService = settingsService;
    }

    public async Task<IActionResult> Index()
    {
        var settings = await _settingsService.GetAsync();

        if (settings.SimpleMode && settings.SimpleModeServiceId.HasValue)
        {
            var simpleService = await _serviceService.GetServiceByIdAsync(settings.SimpleModeServiceId.Value);
            return View("SimpleIndex", simpleService);
        }

        var services = await _serviceService.GetMainServicesAsync();
        if (settings.SimpleModeServiceId.HasValue)
        {
            services = services.Where(s => s.Id != settings.SimpleModeServiceId.Value).ToList();
        }
        return View(services);
    }

    public async Task<IActionResult> Select(Guid id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);

        if (service.Children.Any())
        {
            return View("ChildServices", service);
        }

        if (service.IsNeedFacets)
        {
            return View("Form", service);
        }

        var ticket = await _ticketService.CreateAsync(service.Id, null, service.Letter);

        return View("Ticket", ticket);
    }

    [HttpPost]
    public async Task<IActionResult> GetTicket(Guid serviceId, string? letter, string? phoneNumber, string? fullName)
    {
        string? facets = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber) || !string.IsNullOrWhiteSpace(fullName))
        {
            facets = System.Text.Json.JsonSerializer.Serialize(new { phoneNumber, fullName });
        }
        var ticket = await _ticketService.CreateAsync(serviceId, facets, letter);
        return View("Ticket", ticket);
    }
}
