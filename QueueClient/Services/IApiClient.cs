using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QueueClient.Models;

namespace QueueClient.Services;

/// <summary>
/// Доступ ко всем эндпойнтам REST API. После входа подставляет JWT автоматически.
/// </summary>
public interface IApiClient
{
    // Авторизация
    Task<LoginResponse> LoginAsync(string email, string password);

    // Услуги / киоск
    Task<ServicesResponse> GetServicesAsync();
    Task<ServiceModel> GetServiceAsync(Guid id);
    Task<TicketModel> CreateTicketAsync(CreateTicketRequest request);

    // Табло
    Task<DisplayModel> GetDisplayAsync();

    // Оператор
    Task<OperatorDashboardModel> GetDashboardAsync();
    Task<WindowModel> StartShiftAsync();
    Task<WindowModel> EndShiftAsync();
    Task CallNextAsync();
    Task RecallAsync();
    Task StartProcessingAsync();
    Task CompleteAsync();
    Task CancelAsync();
    Task RedirectAsync(Guid serviceId, string comment);

    // Админ: пользователи
    Task<List<UserModel>> GetUsersAsync();
    Task<UserModel> AddUserAsync(CreateUserDto user, List<string> roles);
    Task<UserModel> EditUserAsync(EditUserDto user, List<string> roles);
    Task RemoveUserAsync(Guid id);

    // Админ: услуги
    Task<List<ServiceModel>> GetAllServicesAsync();
    Task<ServiceModel> AddServiceAsync(CreateServiceDto dto);
    Task ToggleServiceStatusAsync(Guid id);
    Task ToggleServiceFacetsAsync(Guid id);

    // Админ: окна
    Task<List<WindowModel>> GetWindowsAsync();
    Task<WindowModel> CreateWindowAsync(CreateWindowDto dto);

    // Админ: настройки
    Task<List<SettingModel>> GetSettingsAsync();
    Task UpdateSettingAsync(Guid id, string value);

    // Админ: статистика и сброс очереди
    Task<List<TicketModel>> GetStatisticsAsync(DateTime start, DateTime end);
    Task QueueResetAsync();
}
