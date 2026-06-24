using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Models;
using QueueClient.Services;
using QueueClient.ViewModels;

namespace QueueClient.ViewModels.Admin;

public partial class AdminServicesViewModel : ViewModelBase, ILoadable
{
    private readonly IApiClient _api;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;

    [ObservableProperty] private bool _isFormOpen;
    [ObservableProperty] private string _formName = "";
    [ObservableProperty] private string? _formLetter;
    [ObservableProperty] private string? _formDescription;
    [ObservableProperty] private ServiceModel? _formParent;

    public ObservableCollection<ServiceModel> Services { get; } = new();
    /// <summary>Возможные родители (главные услуги) + "нет".</summary>
    public ObservableCollection<ServiceModel> Parents { get; } = new();

    public AdminServicesViewModel(IApiClient api) => _api = api;

    public async Task LoadAsync()
    {
        IsBusy = true; Error = null;
        try
        {
            var all = await _api.GetAllServicesAsync();
            Services.Clear();
            foreach (var s in all) Services.Add(s);

            // В родители — только главные (без ParentId)
            Parents.Clear();
            foreach (var s in all.Where(s => s.ParentId == null)) Parents.Add(s);
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void New()
    {
        FormName = "";
        FormLetter = null;
        FormDescription = null;
        FormParent = null;
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
            await _api.AddServiceAsync(new CreateServiceDto
            {
                Name = FormName,
                Letter = FormLetter,
                Description = FormDescription,
                ParentId = FormParent?.Id
            });
            IsFormOpen = false;
            await LoadAsync();
        }
        catch (Exception ex) { Error = "Не удалось сохранить: " + ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task ToggleStatus(ServiceModel s) => await Do(() => _api.ToggleServiceStatusAsync(s.Id));

    [RelayCommand]
    private async Task ToggleFacets(ServiceModel s) => await Do(() => _api.ToggleServiceFacetsAsync(s.Id));

    private async Task Do(Func<Task> action)
    {
        IsBusy = true; Error = null;
        try { await action(); await LoadAsync(); }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }
}
