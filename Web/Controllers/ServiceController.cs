using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using Application.Interfaces;
using Application.Interfaces.Repositories;
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
        var settingSimpleMode = await _settingsService.GetSettingByNameAsync("Простой режим");
        var settingSimpleModeLetter = await _settingsService.GetSettingByNameAsync("Буква для простого режима");
        var services = await _serviceService.GetMainServicesAsync();

        if (settingSimpleMode?.Value == "true" || !services.Any())
        {
            return View("SimpleIndex", settingSimpleModeLetter);
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
