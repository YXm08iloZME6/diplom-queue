using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Services;

namespace QueueClient.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IApiClient _api;
    private readonly SessionService _session;

    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;

    /// <summary>Поднимается после успешного входа — корневой VM переключает экран.</summary>
    public event Action? LoggedIn;

    public LoginViewModel(IApiClient api, SessionService session)
    {
        _api = api;
        _session = session;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy) return;
        Error = null;
        IsBusy = true;
        try
        {
            var resp = await _api.LoginAsync(Email.Trim(), Password);
            _session.Set(resp.Token, resp.Id, resp.Email, resp.Roles);
            LoggedIn?.Invoke();
        }
        catch (Exception)
        {
            Error = "Не удалось войти. Проверьте email и пароль.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
