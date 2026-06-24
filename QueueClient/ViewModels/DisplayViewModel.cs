using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QueueClient.Models;
using QueueClient.Services;

namespace QueueClient.ViewModels;

/// <summary>Табло в зале: активные вызовы по окнам + очередь ожидания. Живо по SignalR.</summary>
public partial class DisplayViewModel : ViewModelBase, ILoadable, IDisposable
{
    private readonly IApiClient _api;
    private readonly SignalRService _signalR;

    [ObservableProperty] private string? _error;

    public ObservableCollection<DisplayTicketModel> ActiveTickets { get; } = new();
    public ObservableCollection<DisplayTicketModel> WaitingTickets { get; } = new();

    public DisplayViewModel(IApiClient api, SignalRService signalR)
    {
        _api = api;
        _signalR = signalR;
        _signalR.NewTicket += OnChanged;
        _signalR.UpdateTicket += OnChanged;
    }

    private async void OnChanged(TicketModel _) => await LoadAsync();

    public async Task LoadAsync()
    {
        try
        {
            var d = await _api.GetDisplayAsync();

            ActiveTickets.Clear();
            foreach (var a in d.ActiveTickets) ActiveTickets.Add(a);

            WaitingTickets.Clear();
            foreach (var w in d.WaitingTickets) WaitingTickets.Add(w);

            Error = null;
        }
        catch (Exception ex)
        {
            Error = "Нет связи с сервером: " + ex.Message;
        }
    }

    public void Dispose()
    {
        _signalR.NewTicket -= OnChanged;
        _signalR.UpdateTicket -= OnChanged;
    }
}
