using Application.Interfaces;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class DisplayController : Controller
{
    private readonly IDisplayService _displayService;
    
    public DisplayController(IDisplayService displayService)
    {
        _displayService = displayService;
    }
    
    public async Task<IActionResult> Index()
    {
        var data = await  _displayService.GetDisplayDataAsync();
        
        return View(data);
    }
}