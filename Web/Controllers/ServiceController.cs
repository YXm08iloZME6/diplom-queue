using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ServiceController : Controller
{
    private readonly IServiceService _service;
    public ServiceController(IServiceService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> Index()
    {
        var services = await _service.GetMainServicesAsync();
        return View(services);
    }
}