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

    public async Task<IActionResult> ActiveList()
    {
        var data = await _displayService.GetDisplayDataAsync();
        return PartialView("ActiveList", data);
    }

    public async Task<IActionResult> WaitingList()
    {
        var data = await _displayService.GetDisplayDataAsync();
        return PartialView("WaitingList", data);
    }
}