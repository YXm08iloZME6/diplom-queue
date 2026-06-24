using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Models;
using QueueClient.Services;
using QueueClient.ViewModels;

namespace QueueClient.ViewModels.Admin;

public partial class AdminUsersViewModel : ViewModelBase, ILoadable
{
    private readonly IApiClient _api;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;

    // Состояние формы добавления/редактирования
    [ObservableProperty] private bool _isFormOpen;
    [ObservableProperty] private bool _isEditMode;
    private Guid _formId;
    [ObservableProperty] private string? _formName;
    [ObservableProperty] private string? _formSurname;
    [ObservableProperty] private string? _formMiddleName;
    [ObservableProperty] private string _formEmail = "";
    [ObservableProperty] private string _formPassword = "";
    [ObservableProperty] private UserStatus _formStatus;
    [ObservableProperty] private WindowModel? _formWindow;
    [ObservableProperty] private bool _formIsAdmin;

    public ObservableCollection<UserModel> Users { get; } = new();
    public ObservableCollection<WindowModel> Windows { get; } = new();
    public UserStatus[] Statuses { get; } = Enum.GetValues<UserStatus>();

    public AdminUsersViewModel(IApiClient api) => _api = api;

    public async Task LoadAsync()
    {
        IsBusy = true; Error = null;
        try
        {
            Users.Clear();
            foreach (var u in await _api.GetUsersAsync()) Users.Add(u);

            Windows.Clear();
            foreach (var w in await _api.GetWindowsAsync()) Windows.Add(w);
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void New()
    {
        IsEditMode = false;
        _formId = Guid.Empty;
        FormName = FormSurname = FormMiddleName = null;
        FormEmail = "";
        FormPassword = "";
        FormStatus = UserStatus.Waiting;
        FormWindow = null;
        FormIsAdmin = false;
        IsFormOpen = true;
    }

    [RelayCommand]
    private void Edit(UserModel user)
    {
        IsEditMode = true;
        _formId = user.Id;
        FormName = user.Name;
        FormSurname = user.Surname;
        FormMiddleName = user.MiddleName;
        FormEmail = user.Email;
        FormPassword = "";
        FormStatus = user.Status;
        FormWindow = Windows.FirstOrDefault(w => w.Id == user.WindowId);
        FormIsAdmin = user.Roles.Contains("admin", StringComparer.OrdinalIgnoreCase);
        IsFormOpen = true;
    }

    [RelayCommand]
    private void CloseForm() => IsFormOpen = false;

    [RelayCommand]
    private async Task Save()
    {
        IsBusy = true; Error = null;
        try
        {
            var roles = new List<string> { "operator" };
            if (FormIsAdmin) roles.Add("admin");

            if (IsEditMode)
            {
                var dto = new EditUserDto
                {
                    Id = _formId,
                    Name = FormName,
                    Surname = FormSurname,
                    MiddleName = FormMiddleName,
                    Email = FormEmail,
                    Password = string.IsNullOrWhiteSpace(FormPassword) ? null : FormPassword,
                    Status = FormStatus,
                    WindowId = FormWindow?.Id,
                    Roles = roles
                };
                await _api.EditUserAsync(dto, roles);
            }
            else
            {
                var dto = new CreateUserDto
                {
                    Name = FormName,
                    Surname = FormSurname,
                    MiddleName = FormMiddleName,
                    Email = FormEmail,
                    Password = FormPassword,
                    Status = FormStatus,
                    WindowId = FormWindow?.Id
                };
                await _api.AddUserAsync(dto, roles);
            }

            IsFormOpen = false;
            await LoadAsync();
        }
        catch (Exception ex) { Error = "Не удалось сохранить: " + ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task Remove(UserModel user)
    {
        IsBusy = true; Error = null;
        try
        {
            await _api.RemoveUserAsync(user.Id);
            await LoadAsync();
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }
}
