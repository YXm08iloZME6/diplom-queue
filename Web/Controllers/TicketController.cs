using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : Controller
{
    private readonly ITicketService  _ticketService;
    
    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> CreateTicket(
        Guid serviceId)
    {
        TicketDto ticket = await _ticketService.CreateTicketAsync(serviceId);
        return Ok(ticket);
    }
}