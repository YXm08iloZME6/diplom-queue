using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Models;
using QueueClient.Services;
using QueueClient.ViewModels;

namespace QueueClient.ViewModels.Admin;

/// <summary>Одна настройка с редактируемым значением.</summary>
public partial class SettingItem : ObservableObject
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public TypeOfSettingsValue Type { get; init; }

    [ObservableProperty] private string _value = "";

    public bool IsBool => Type == TypeOfSettingsValue.Bool;
    public bool IsInt => Type == TypeOfSettingsValue.Int;
    public bool IsString => Type == TypeOfSettingsValue.String;

    /// <summary>Для bool-настроек: удобно биндить на ToggleSwitch.</summary>
    public bool BoolValue
    {
        get => Value == "true";
        set => Value = value ? "true" : "false";
    }
}

public partial class AdminSettingsViewModel : ViewModelBase, ILoadable
{
    private readonly IApiClient _api;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;
    [ObservableProperty] private string? _info;

    public ObservableCollection<SettingItem> Settings { get; } = new();

    public AdminSettingsViewModel(IApiClient api) => _api = api;

    public async Task LoadAsync()
    {
        IsBusy = true; Error = null;
        try
        {
            Settings.Clear();
            foreach (var s in await _api.GetSettingsAsync())
            {
                Settings.Add(new SettingItem
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Type = s.TypeOfSettingsValue,
                    Value = s.Value
                });
            }
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task Save(SettingItem item)
    {
        IsBusy = true; Error = null; Info = null;
        try
        {
            await _api.UpdateSettingAsync(item.Id, item.Value);
            Info = $"«{item.Name}» сохранено";
        }
        catch (Exception ex) { Error = "Не удалось сохранить: " + ex.Message; }
        finally { IsBusy = false; }
    }
}
