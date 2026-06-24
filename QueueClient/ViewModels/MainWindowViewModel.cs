using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QueueClient.Configuration;
using QueueClient.Services;
using QueueClient.ViewModels.Admin;

namespace QueueClient.ViewModels;

/// <summary>
/// Корневой VM. Определяет, что показать при старте, исходя из режима (App.Mode)
/// и роли вошедшего пользователя. Также подсказывает окну, разворачиваться ли на весь экран.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly AppOptions _options;
    private readonly IApiClient _api;
    private readonly SessionService _session;
    private readonly IViewModelFactory _factory;
    private readonly SignalRService _signalR;

    [ObservableProperty] private ViewModelBase? _currentPage;

    /// <summary>true для Kiosk/Display — окно на весь экран без рамок.</summary>
    [ObservableProperty] private bool _isFullScreen;

    public MainWindowViewModel(
        AppOptions options,
        IApiClient api,
        SessionService session,
        IViewModelFactory factory,
        SignalRService signalR)
    {
        _options = options;
        _api = api;
        _session = session;
        _factory = factory;
        _signalR = signalR;
    }

    /// <summary>Точка старта приложения (вызывается из App).</summary>
    public async Task InitializeAsync()
    {
        switch (_options.Mode)
        {
            case AppMode.Kiosk:
                await ServiceLoginAsync();
                await _signalR.StartAsync();
                IsFullScreen = true;
                CurrentPage = _factory.Create<KioskViewModel>();
                break;

            case AppMode.Display:
                await ServiceLoginAsync();
                await _signalR.StartAsync();
                IsFullScreen = true;
                var display = _factory.Create<DisplayViewModel>();
                CurrentPage = display;
                await display.LoadAsync();
                break;

            default: // Login
                IsFullScreen = false;
                var login = _factory.Create<LoginViewModel>();
                login.LoggedIn += OnLoggedIn;
                CurrentPage = login;
                break;
        }
    }

    /// <summary>Авто-вход сервисной учёткой для терминала/табло.</summary>
    private async Task ServiceLoginAsync()
    {
        var resp = await _api.LoginAsync(_options.ServiceAccountLogin, _options.ServiceAccountPassword);
        _session.Set(resp.Token, resp.Id, resp.Email, resp.Roles);
    }

    private async void OnLoggedIn()
    {
        await _signalR.StartAsync();
        // По роли выбираем рабочий раздел. Админ видит всё (включая рабочее место).
        var shell = _factory.Create<ShellViewModel>();
        shell.LogoutRequested += OnLoggedOut;
        await shell.InitializeAsync();
        CurrentPage = shell;
    }

    private void OnLoggedOut()
    {
        // Возврат на экран входа
        var login = _factory.Create<LoginViewModel>();
        login.LoggedIn += OnLoggedIn;
        CurrentPage = login;
    }
}
