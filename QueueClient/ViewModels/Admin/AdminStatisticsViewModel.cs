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

public partial class AdminStatisticsViewModel : ViewModelBase, ILoadable
{
    private readonly IApiClient _api;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;
    [ObservableProperty] private string? _info;

    [ObservableProperty] private DateTime? _startDate = DateTime.Today;
    [ObservableProperty] private DateTime? _endDate = DateTime.Today.AddDays(1);

    [ObservableProperty] private int _total;
    [ObservableProperty] private int _completed;
    [ObservableProperty] private int _cancelled;

    public ObservableCollection<TicketModel> Tickets { get; } = new();

    public AdminStatisticsViewModel(IApiClient api) => _api = api;

    // При открытии раздела сразу грузим за сегодня
    public Task LoadAsync() => Search();

    [RelayCommand]
    private async Task Search()
    {
        IsBusy = true; Error = null;
        try
        {
            var start = StartDate ?? DateTime.Today;
            var end = EndDate ?? DateTime.Today.AddDays(1);
            var list = await _api.GetStatisticsAsync(start, end);
            Tickets.Clear();
            foreach (var t in list) Tickets.Add(t);

            Total = list.Count;
            Completed = list.Count(t => t.Status == nameof(TicketStatus.Completed));
            Cancelled = list.Count(t => t.Status == nameof(TicketStatus.Cancelled));
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task ResetQueue()
    {
        IsBusy = true; Error = null; Info = null;
        try
        {
            await _api.QueueResetAsync();
            Info = "Очередь успешно сброшена";
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }
}
