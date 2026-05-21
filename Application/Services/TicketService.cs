using Application.Interfaces;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repository; 
    
    public TicketService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketDto> CreateAsync(Guid serviceId, string? info, string? letter)
    {
        var count = await _repository.GetTicketCountAsync(letter);

        var ticket = new Ticket
        {
            ServiceId = serviceId,
            CreatedAt = DateTime.UtcNow,
            Number = $"{letter}-{(count + 1):D3}",
            Facets = info
        };

        await _repository.AddAsync(ticket);

        return new TicketDto(ticket);
    }

    public async Task<TicketDto?> GetByIdAsync(Guid id)
    {
        var ticket = await _repository.GetByIdAsync(id);
        return ticket == null ? null : new TicketDto(ticket);
    }

    public async Task<List<TicketDto>> GetAllAsync()
    {
        var tickets = await _repository.GetAllAsync();
        return tickets.Select(t => new TicketDto(t)).ToList();
    }

    public async Task<TicketDto> UpdateAsync(Guid ticketId, UpdateTicketDto dto)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception($"Талон {ticketId} не найден");
        
        if (dto.ServiceId.HasValue) ticket.ServiceId = dto.ServiceId;
        if (dto.WindowId.HasValue) ticket.WindowId = dto.WindowId;
        if (dto.Number != null) ticket.Number = dto.Number;
        if (dto.Facets != null) ticket.Facets = dto.Facets;
        if (dto.RedirectComment != null) ticket.RedirectComment = dto.RedirectComment;

        await _repository.UpdateAsync(ticket);

        return new TicketDto(ticket);
    }

    public async Task<bool> DeleteAsync(Guid ticketId)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null) return false;

        await _repository.DeleteAsync(ticket);
        return true;
    }

    public async Task<TicketDto> CallAsync(Guid ticketId, Guid windowId)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception($"Талон {ticketId} не найден");
        
        ticket.WindowId = windowId;
        ticket.CalledAt = DateTime.UtcNow;
        ticket.Status = TicketStatus.Called;

        await _repository.UpdateAsync(ticket);
        return new TicketDto(ticket);
    }

    public async Task<TicketDto> RecallAsync(Guid ticketId)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception($"Талон {ticketId} не найден");

        ticket.CalledAt = DateTime.UtcNow;

        await _repository.UpdateAsync(ticket);
        return new TicketDto(ticket);
    }

    public async Task<TicketDto> StartProcessingAsync(Guid ticketId)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception($"Талон {ticketId} не найден");
        
        ticket.StartedAt = DateTime.UtcNow;
        ticket.Status = TicketStatus.Processing;

        await _repository.UpdateAsync(ticket);
        return new TicketDto(ticket);
    }

    public async Task<TicketDto> CompleteAsync(Guid ticketId)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception($"Талон {ticketId} не найден");
        
        ticket.CompletedAt = DateTime.UtcNow;
        ticket.Status = TicketStatus.Completed;

        await _repository.UpdateAsync(ticket);
        return new TicketDto(ticket);
    }

    public async Task<TicketDto> CancelAsync(Guid ticketId)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception($"Талон {ticketId} не найден");
        

        ticket.CompletedAt = DateTime.UtcNow;
        ticket.Status = TicketStatus.Cancelled;

        await _repository.UpdateAsync(ticket);
        return new TicketDto(ticket);
    }

    public async Task<TicketDto> RedirectAsync(Guid ticketId, Guid newServiceId, string comment)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception($"Талон {ticketId} не найден");
        
        ticket.ServiceId = newServiceId;
        ticket.WindowId = null;
        ticket.CalledAt = null;
        ticket.StartedAt = null;
        ticket.RedirectComment = comment;
        ticket.Status = TicketStatus.Waiting;

        await _repository.UpdateAsync(ticket);
        return new TicketDto(ticket);
    }
}