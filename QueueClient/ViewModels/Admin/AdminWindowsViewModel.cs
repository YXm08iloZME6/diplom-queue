using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Models;
using QueueClient.Services;
using QueueClient.ViewModels;

namespace QueueClient.ViewModels.Admin;

public partial class AdminWindowsViewModel : ViewModelBase, ILoadable
{
    private readonly IApiClient _api;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;

    [ObservableProperty] private bool _isFormOpen;
    [ObservableProperty] private string? _formNumber;
    [ObservableProperty] private WindowStatus _formStatus;
    [ObservableProperty] private ServiceModel? _formService;

    public ObservableCollection<WindowModel> Windows { get; } = new();
    public ObservableCollection<ServiceModel> Services { get; } = new();
    public WindowStatus[] Statuses { get; } = Enum.GetValues<WindowStatus>();

    public AdminWindowsViewModel(IApiClient api) => _api = api;

    public async Task LoadAsync()
    {
        IsBusy = true; Error = null;
        try
        {
            Windows.Clear();
            foreach (var w in await _api.GetWindowsAsync()) Windows.Add(w);

            Services.Clear();
            foreach (var s in await _api.GetAllServicesAsync()) Services.Add(s);
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void New()
    {
        FormNumber = null;
        FormStatus = WindowStatus.Close;
        FormService = null;
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
            await _api.CreateWindowAsync(new CreateWindowDto
            {
                Number  = FormNumber,
                Status = FormStatus,
                Service = FormService!,
                ServiceId = FormService?.Id
            });
            IsFormOpen = false;
            await LoadAsync();
        }
        catch (Exception ex) { Error = "Не удалось сохранить: " + ex.Message; }
        finally { IsBusy = false; }
    }
}
