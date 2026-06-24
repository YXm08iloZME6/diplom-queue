using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Models;
using QueueClient.Services;

namespace QueueClient.ViewModels;

/// <summary>
/// Рабочее место оператора. Кнопки повторяют веб-логику по статусу талона:
///   нет талона        → «Вызвать следующего»;
///   Called            → «Начать обслуживание» / «Повторить» / «Отменить»;
///   Processing        → «Завершить» / «Перенаправить».
/// Обновляется вживую по событиям SignalR.
/// </summary>
public partial class OperatorViewModel : ViewModelBase, ILoadable, IDisposable
{
    private readonly IApiClient _api;
    private readonly SignalRService _signalR;

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;
    [ObservableProperty] private string? _info;

    [ObservableProperty] private string _windowTitle = "—";
    [ObservableProperty] private string _serviceName = "—";

    [ObservableProperty] private TicketModel? _currentTicket;
    [ObservableProperty] private int _waitingCount;

    [ObservableProperty] private ServiceModel? _redirectTarget;
    [ObservableProperty] private string _redirectComment = "";

    // Производные флаги для показа кнопок
    public bool HasTicket => CurrentTicket != null;
    public bool IsCalled => CurrentTicket?.Status == nameof(TicketStatus.Called);
    public bool IsProcessing => CurrentTicket?.Status == nameof(TicketStatus.Processing);

    // Комментарий от перенаправившего оператора (если талон пришёл по redirect)
    public bool HasRedirectComment => !string.IsNullOrWhiteSpace(CurrentTicket?.RedirectComment);
    public string? CurrentRedirectComment => CurrentTicket?.RedirectComment;

    public ObservableCollection<TicketModel> WaitingTickets { get; } = new();
    public ObservableCollection<ServiceModel> AllServices { get; } = new();

    public OperatorViewModel(IApiClient api, SignalRService signalR)
    {
        _api = api;
        _signalR = signalR;
        _signalR.NewTicket += OnQueueChanged;
        _signalR.UpdateTicket += OnQueueChanged;
    }

    private async void OnQueueChanged(TicketModel _) => await LoadAsync();

    public async Task LoadAsync()
    {
        IsBusy = true;
        Error = null;
        try
        {
            var d = await _api.GetDashboardAsync();

            WindowTitle = d.Window?.Number ?? "—";
            ServiceName = d.Window?.ServiceName ?? "Общая очередь";
            CurrentTicket = d.CurrentTicket;
            WaitingCount = d.WaitingCount;

            WaitingTickets.Clear();
            foreach (var t in d.WaitingTickets) WaitingTickets.Add(t);

            AllServices.Clear();
            foreach (var s in d.AllServices) AllServices.Add(s);

            NotifyTicketFlags();
        }
        catch (Exception ex)
        {
            // Чаще всего — смена не начата (окно не назначено). Подсказываем начать смену.
            Error = "Не удалось загрузить панель. Возможно, нужно начать смену. " + ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void NotifyTicketFlags()
    {
        OnPropertyChanged(nameof(HasTicket));
        OnPropertyChanged(nameof(IsCalled));
        OnPropertyChanged(nameof(IsProcessing));
        OnPropertyChanged(nameof(HasRedirectComment));
        OnPropertyChanged(nameof(CurrentRedirectComment));
    }

    [RelayCommand]
    private Task StartShift() => RunWithMessage(
        async () => { await _api.StartShiftAsync(); }, "Смена начата — окно открыто");

    [RelayCommand]
    private Task EndShift() => RunWithMessage(
        async () => { await _api.EndShiftAsync(); }, "Смена завершена — окно закрыто");
    [RelayCommand] private Task CallNext() => Run(_api.CallNextAsync);
    [RelayCommand] private Task Recall() => Run(_api.RecallAsync);
    [RelayCommand] private Task StartProcessing() => Run(_api.StartProcessingAsync);
    [RelayCommand] private Task Complete() => Run(_api.CompleteAsync);
    [RelayCommand] private Task Cancel() => Run(_api.CancelAsync);
    [RelayCommand] private Task Refresh() => LoadAsync();

    [RelayCommand]
    private async Task Redirect()
    {
        if (RedirectTarget is null)
        {
            Error = "Сначала выберите услугу для перенаправления.";
            return;
        }
        await Run(() => _api.RedirectAsync(RedirectTarget.Id, RedirectComment));
        RedirectComment = "";
        RedirectTarget = null;
    }

    // Обёртка: выполнить действие и перечитать панель
    private async Task Run(Func<Task> action)
    {
        IsBusy = true;
        Error = null;
        Info = null;
        try
        {
            await action();
            await LoadAsync();
        }
        catch (Exception ex)
        {
            Error = "Ошибка: " + ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Как Run, но при успехе показывает сообщение (для начала/конца смены)
    private async Task RunWithMessage(Func<Task> action, string successMessage)
    {
        IsBusy = true;
        Error = null;
        Info = null;
        try
        {
            await action();
            await LoadAsync();
            Info = successMessage;
        }
        catch (Exception ex)
        {
            Error = "Ошибка: " + ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void Dispose()
    {
        _signalR.NewTicket -= OnQueueChanged;
        _signalR.UpdateTicket -= OnQueueChanged;
    }
}
