using System;
using System.Collections.Generic;

namespace QueueClient.Models;

// Модели зеркалят DTO сервера (Application.DTOs). JSON настроен без учёта регистра.

// ─── Авторизация ────────────────────────────────────────────

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginResponse
{
    public string Token { get; set; } = "";
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
}

// ─── Услуги (ServiceDto) ────────────────────────────────────

public class ServiceModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string? Letter { get; set; }
    public List<ServiceModel> Children { get; set; } = new();
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public bool IsActive { get; set; }
    public bool IsNeedFacets { get; set; }
    public Guid? ParentId { get; set; }
}

/// <summary>Ответ GET /api/service — обёртка с учётом простого режима.</summary>
public class ServicesResponse
{
    public bool SimpleMode { get; set; }
    public string? Letter { get; set; }
    public List<ServiceModel>? Services { get; set; }
}

public class CreateServiceDto
{
    public string Name { get; set; } = "";
    public string? Letter { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public Guid? ParentId { get; set; }
}

// ─── Талоны (TicketDto) ─────────────────────────────────────

public class TicketModel
{
    public Guid Id { get; set; }
    public string Number { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
    public DateTime? CalledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? WindowId { get; set; }
    public string? RedirectComment { get; set; }
}

/// <summary>Тело POST /api/service/ticket.</summary>
public class CreateTicketRequest
{
    public Guid ServiceId { get; set; }
    public string? Letter { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
}

// ─── Окна (WindowDto) ───────────────────────────────────────

public class WindowModel
{
    public Guid Id { get; set; }
    public string? Number { get; set; }
    public WindowStatus Status { get; set; }
    public Guid? ServiceId { get; set; }
    public ServiceModel? Service { get; set; }
    public string? ServiceName { get; set; }
}

public class CreateWindowDto
{
    public string? Number { get; set; }
    public WindowStatus Status { get; set; }
    public ServiceModel Service { get; set; }
    public Guid? ServiceId { get; set; }
}

// ─── Пользователи (UserDto) ─────────────────────────────────

public class UserModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public UserStatus Status { get; set; }
    public string Email { get; set; } = "";
    public Guid? WindowId { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class CreateUserDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public UserStatus Status { get; set; }
    public Guid? WindowId { get; set; }
}

public class EditUserDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; } = "";
    public string? Password { get; set; }
    public UserStatus Status { get; set; }
    public Guid? WindowId { get; set; }
    public List<string> Roles { get; set; } = new();
}

// Тела составных запросов админки (см. AdminController)
public record AddUserRequest(CreateUserDto UserData, List<string> RoleNames);
public record EditUserRequest(EditUserDto UserData, List<string> RoleNames);

// ─── Настройки (SettingsDto) ────────────────────────────────

public class SettingModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
    public string Description { get; set; } = "";
    public TypeOfSettingsValue TypeOfSettingsValue { get; set; }
}

// ─── Табло (DisplayDto) ─────────────────────────────────────

public class DisplayModel
{
    public List<DisplayTicketModel> ActiveTickets { get; set; } = new();
    public List<DisplayTicketModel> WaitingTickets { get; set; } = new();
}

public class DisplayTicketModel
{
    public Guid? WindowId { get; set; }
    public string? Title { get; set; } = "";
    public TicketStatus? Status { get; set; }
    public string? TicketNumber { get; set; }
    public string? WindowNumber { get; set; } = "";
    public string? ServiceName { get; set; } = "";
}

// ─── Панель оператора (OperatorDashboardDto) ────────────────

public class OperatorDashboardModel
{
    public WindowModel? Window { get; set; }
    public TicketModel? CurrentTicket { get; set; }
    public int WaitingCount { get; set; }
    public List<TicketModel> WaitingTickets { get; set; } = new();
    public List<ServiceModel> AllServices { get; set; } = new();
}

// ─── Перенаправление (тело POST /api/operator/redirect) ─────

public record RedirectRequest(Guid ServiceId, string Comment);
