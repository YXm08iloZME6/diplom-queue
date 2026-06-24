using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IWindowService _windowService;
    private readonly IServiceService _serviceService;
    private readonly ISettingsService _settingsService;

    public AdminController(
        IAdminService adminService,
        IWindowService windowService,
        IServiceService serviceService,
        ISettingsService settingsService)
    {
        _adminService = adminService;
        _windowService = windowService;
        _serviceService = serviceService;
        _settingsService = settingsService;
    }

    // ─── Пользователи ───────────────────────────────────────

    /// <summary>Список всех пользователей</summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _adminService.GetAllUsers();
        return Ok(users);
    }

    /// <summary>Получить пользователя по id</summary>
    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _adminService.GetUserById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>Добавить пользователя</summary>
    [HttpPost("users")]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
    {
        var user = await _adminService.AddUser(request.UserData, request.RoleNames);
        return Ok(user);
    }

    /// <summary>Редактировать пользователя</summary>
    [HttpPut("users")]
    public async Task<IActionResult> EditUser([FromBody] EditUserRequest request)
    {
        var user = await _adminService.EditUser(request.UserData, request.RoleNames);
        return Ok(user);
    }

    /// <summary>Удалить пользователя</summary>
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> RemoveUser(Guid id)
    {
        await _adminService.RemoveUser(id);
        return Ok();
    }

    // ─── Услуги ─────────────────────────────────────────────

    /// <summary>Список всех услуг</summary>
    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var services = await _serviceService.GetAllServicesAsync();
        return Ok(services);
    }

    /// <summary>Добавить услугу</summary>
    [HttpPost("services")]
    public async Task<IActionResult> AddService([FromBody] CreateServiceDto dto)
    {
        var service = await _adminService.AddServiceAsync(dto);
        return Ok(service);
    }

    /// <summary>Включить/выключить услугу</summary>
    [HttpPost("services/{id}/toggle-status")]
    public async Task<IActionResult> ToggleServiceStatus(Guid id)
    {
        await _adminService.ToggleServiceStatus(id);
        return Ok();
    }

    /// <summary>Включить/выключить сбор данных (facets) для услуги</summary>
    [HttpPost("services/{id}/toggle-facets")]
    public async Task<IActionResult> ToggleServiceFacets(Guid id)
    {
        await _adminService.ToggleServiceFacets(id);
        return Ok();
    }

    // ─── Окна ───────────────────────────────────────────────

    /// <summary>Список всех окон</summary>
    [HttpGet("windows")]
    public async Task<IActionResult> GetWindows()
    {
        var windows = await _windowService.GetAllWindows();
        return Ok(windows);
    }

    /// <summary>Создать окно</summary>
    [HttpPost("windows")]
    public async Task<IActionResult> CreateWindow([FromBody] CreateWindowDto dto)
    {
        var window = await _windowService.CreateWindowAsync(dto);
        return Ok(window);
    }

    // ─── Очередь ────────────────────────────────────────────

    /// <summary>Сбросить очередь</summary>
    [HttpPost("queue-reset")]
    public async Task<IActionResult> QueueReset()
    {
        await _adminService.QueueResetAsync();
        return Ok(new { message = "Очередь успешно сброшена" });
    }

    /// <summary>Статистика по талонам за период</summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var tickets = await _adminService.TicketStats(startDate, endDate);
        return Ok(tickets);
    }

    // ─── Настройки ──────────────────────────────────────────

    /// <summary>Получить все настройки</summary>
    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        var settings = await _settingsService.GetSettingsAsync();
        return Ok(settings);
    }

    /// <summary>Обновить значение настройки</summary>
    [HttpPut("settings/{id}")]
    public async Task<IActionResult> UpdateSetting(Guid id, [FromBody] UpdateSettingRequest request)
    {
        await _settingsService.UpdateSettingValueAsync(id, request.Value);
        return Ok();
    }
}

public record AddUserRequest(CreateUserDto UserData, List<string> RoleNames);
public record EditUserRequest(EditUserDto UserData, List<string> RoleNames);
public record UpdateSettingRequest(string Value);