using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using QueueClient.Models;

namespace QueueClient.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly SessionService _session;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient http, SessionService session)
    {
        _http = http;
        _session = session;
    }

    // ─── Авторизация ────────────────────────────────────────

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var body = new LoginRequest { Email = email, Password = password };
        using var resp = await _http.PostAsJsonAsync("api/auth/login", body, JsonOpts);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts)
               ?? throw new InvalidOperationException("Пустой ответ логина");
    }

    // ─── Услуги / киоск ─────────────────────────────────────

    public Task<ServicesResponse> GetServicesAsync()
        => GetAsync<ServicesResponse>("api/service");

    public Task<ServiceModel> GetServiceAsync(Guid id)
        => GetAsync<ServiceModel>($"api/service/{id}");

    public Task<TicketModel> CreateTicketAsync(CreateTicketRequest request)
        => PostAsync<TicketModel>("api/service/ticket", request);

    // ─── Табло ──────────────────────────────────────────────

    public Task<DisplayModel> GetDisplayAsync()
        => GetAsync<DisplayModel>("api/display");

    // ─── Оператор ───────────────────────────────────────────

    public Task<OperatorDashboardModel> GetDashboardAsync()
        => GetAsync<OperatorDashboardModel>("api/operator/dashboard");

    public Task<WindowModel> StartShiftAsync()
        => PostAsync<WindowModel>("api/operator/start-shift", null);

    public Task<WindowModel> EndShiftAsync()
        => PostAsync<WindowModel>("api/operator/end-shift", null);

    public Task CallNextAsync() => PostVoidAsync("api/operator/call-next");
    public Task RecallAsync() => PostVoidAsync("api/operator/recall");
    public Task StartProcessingAsync() => PostVoidAsync("api/operator/start");
    public Task CompleteAsync() => PostVoidAsync("api/operator/complete");
    public Task CancelAsync() => PostVoidAsync("api/operator/cancel");

    public Task RedirectAsync(Guid serviceId, string comment)
        => PostVoidAsync("api/operator/redirect", new RedirectRequest(serviceId, comment));

    // ─── Админ: пользователи ────────────────────────────────

    public Task<List<UserModel>> GetUsersAsync()
        => GetAsync<List<UserModel>>("api/admin/users");

    public Task<UserModel> AddUserAsync(CreateUserDto user, List<string> roles)
        => PostAsync<UserModel>("api/admin/users", new AddUserRequest(user, roles));

    public Task<UserModel> EditUserAsync(EditUserDto user, List<string> roles)
        => PutAsync<UserModel>("api/admin/users", new EditUserRequest(user, roles));

    public Task RemoveUserAsync(Guid id) => DeleteVoidAsync($"api/admin/users/{id}");

    // ─── Админ: услуги ──────────────────────────────────────

    public Task<List<ServiceModel>> GetAllServicesAsync()
        => GetAsync<List<ServiceModel>>("api/admin/services");

    public Task<ServiceModel> AddServiceAsync(CreateServiceDto dto)
        => PostAsync<ServiceModel>("api/admin/services", dto);

    public Task ToggleServiceStatusAsync(Guid id)
        => PostVoidAsync($"api/admin/services/{id}/toggle-status");

    public Task ToggleServiceFacetsAsync(Guid id)
        => PostVoidAsync($"api/admin/services/{id}/toggle-facets");

    // ─── Админ: окна ────────────────────────────────────────

    public Task<List<WindowModel>> GetWindowsAsync()
        => GetAsync<List<WindowModel>>("api/admin/windows");

    public Task<WindowModel> CreateWindowAsync(CreateWindowDto dto)
        => PostAsync<WindowModel>("api/admin/windows", dto);

    // ─── Админ: настройки ───────────────────────────────────

    public Task<List<SettingModel>> GetSettingsAsync()
        => GetAsync<List<SettingModel>>("api/admin/settings");

    public Task UpdateSettingAsync(Guid id, string value)
        => PutVoidAsync($"api/admin/settings/{id}", new { Value = value });

    // ─── Админ: статистика / сброс ──────────────────────────

    public Task<List<TicketModel>> GetStatisticsAsync(DateTime start, DateTime end)
        => GetAsync<List<TicketModel>>(
            $"api/admin/statistics?startDate={start:yyyy-MM-ddTHH:mm:ss}&endDate={end:yyyy-MM-ddTHH:mm:ss}");

    public Task QueueResetAsync() => PostVoidAsync("api/admin/queue-reset");

    // ─── Низкоуровневые помощники ───────────────────────────

    private async Task<T> GetAsync<T>(string url)
    {
        using var resp = await SendAsync(HttpMethod.Get, url, null);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>(JsonOpts)
               ?? throw new InvalidOperationException($"Пустой ответ {url}");
    }

    private async Task<T> PostAsync<T>(string url, object? body)
    {
        using var resp = await SendAsync(HttpMethod.Post, url, body);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>(JsonOpts)
               ?? throw new InvalidOperationException($"Пустой ответ {url}");
    }

    private async Task<T> PutAsync<T>(string url, object? body)
    {
        using var resp = await SendAsync(HttpMethod.Put, url, body);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>(JsonOpts)
               ?? throw new InvalidOperationException($"Пустой ответ {url}");
    }

    private async Task PostVoidAsync(string url, object? body = null)
    {
        using var resp = await SendAsync(HttpMethod.Post, url, body);
        resp.EnsureSuccessStatusCode();
    }

    private async Task PutVoidAsync(string url, object? body)
    {
        using var resp = await SendAsync(HttpMethod.Put, url, body);
        resp.EnsureSuccessStatusCode();
    }

    private async Task DeleteVoidAsync(string url)
    {
        using var resp = await SendAsync(HttpMethod.Delete, url, null);
        resp.EnsureSuccessStatusCode();
    }

    private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, object? body)
    {
        var request = new HttpRequestMessage(method, url);
        if (_session.Token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _session.Token);
        if (body is not null)
            request.Content = JsonContent.Create(body, body.GetType(), options: JsonOpts);
        return await _http.SendAsync(request);
    }
}
