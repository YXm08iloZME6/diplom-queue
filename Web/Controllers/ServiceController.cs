using Application.Interfaces;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ServiceController : Controller
{
    private readonly IServiceService _serviceService;
    private readonly ITicketService _ticketService;
    
    public ServiceController(IServiceService serviceService,  ITicketService ticketService)
    {
        _serviceService = serviceService;
        _ticketService = ticketService;
    }
    
    public async Task<IActionResult> Index()
    {
        var services = await _serviceService.GetMainServicesAsync();
        return View(services);
    }

    public async Task<IActionResult> Select(Guid id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);

        if (service.Children.Any())
        {
            return View("ChildServices", service);
        }

        return View("Form", service);
    }

    [HttpPost]
    public async Task<IActionResult> GetTicket(Guid serviceId, string letter, string phoneNumber, string fullName )
    {
        var facets = System.Text.Json.JsonSerializer.Serialize(new {phoneNumber, fullName});
        var ticket = await _ticketService.CreateAsync(serviceId, facets, letter);
        return View("Ticket", ticket);
    }
}