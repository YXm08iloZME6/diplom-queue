using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Configuration;
using QueueClient.Models;
using QueueClient.Services;

namespace QueueClient.ViewModels;

public enum KioskStep { Welcome, Services, Facets, Ticket }

/// <summary>
/// Терминал выдачи талонов. Внутри — пошаговый сценарий:
/// приветствие → выбор услуги (с подкатегориями) → ввод данных → талон → авто-возврат.
/// </summary>
public partial class KioskViewModel : ViewModelBase
{
    private readonly IApiClient _api;
    private readonly AppOptions _options;

    [ObservableProperty] private KioskStep _step = KioskStep.Welcome;
    [ObservableProperty] private bool _isBusy;

    // Производные флаги для показа нужного экрана
    public bool IsWelcome => Step == KioskStep.Welcome;
    public bool IsServices => Step == KioskStep.Services;
    public bool IsFacets => Step == KioskStep.Facets;
    public bool IsTicket => Step == KioskStep.Ticket;

    partial void OnStepChanged(KioskStep value)
    {
        OnPropertyChanged(nameof(IsWelcome));
        OnPropertyChanged(nameof(IsServices));
        OnPropertyChanged(nameof(IsFacets));
        OnPropertyChanged(nameof(IsTicket));
    }

    [ObservableProperty] private string? _error;
    [ObservableProperty] private string _listTitle = "Выберите услугу";

    // Простой режим
    [ObservableProperty] private bool _isSimpleMode;
    private string? _simpleLetter;

    // Экран ввода данных
    [ObservableProperty] private string _phoneNumber = "";
    [ObservableProperty] private string _fullName = "";
    private ServiceModel? _facetsService;

    // Экран талона
    [ObservableProperty] private string _ticketNumber = "";
    [ObservableProperty] private int _secondsLeft;

    public ObservableCollection<ServiceModel> Services { get; } = new();
    private readonly Stack<List<ServiceModel>> _backStack = new();

    private DispatcherTimer? _idleTimer;
    private DispatcherTimer? _ticketTimer;

    public KioskViewModel(IApiClient api, AppOptions options)
    {
        _api = api;
        _options = options;
    }

    // ─── Приветствие ────────────────────────────────────────

    [RelayCommand]
    private async Task Start()
    {
        await LoadRootServicesAsync();
    }

    private async Task LoadRootServicesAsync()
    {
        IsBusy = true;
        Error = null;
        _backStack.Clear();
        try
        {
            var resp = await _api.GetServicesAsync();
            if (resp.SimpleMode)
            {
                IsSimpleMode = true;
                _simpleLetter = resp.Letter;
            }
            else
            {
                IsSimpleMode = false;
                FillServices(resp.Services ?? new());
            }
            ListTitle = "Выберите услугу";
            Step = KioskStep.Services;
            StartIdleTimer();
        }
        catch (Exception ex)
        {
            Error = "Не удалось загрузить услуги: " + ex.Message;
        }
        finally { IsBusy = false; }
    }

    // ─── Выбор услуги ───────────────────────────────────────

    [RelayCommand]
    private async Task Select(ServiceModel service)
    {
        ResetIdle();
        IsBusy = true;
        Error = null;
        try
        {
            var full = await _api.GetServiceAsync(service.Id);

            if (full.Children is { Count: > 0 })
            {
                _backStack.Push(new List<ServiceModel>(Services));
                FillServices(full.Children);
                ListTitle = full.Name;
            }
            else if (full.IsNeedFacets)
            {
                _facetsService = full;
                PhoneNumber = "";
                FullName = "";
                Step = KioskStep.Facets;
            }
            else
            {
                await CreateTicketAsync(full.Id, full.Letter, null, null);
            }
        }
        catch (Exception ex)
        {
            Error = "Ошибка: " + ex.Message;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SimpleTicket()
    {
        ResetIdle();
        // В простом режиде талон по букве (см. серверную логику).
        await CreateTicketAsync(Guid.Empty, _simpleLetter, null, null);
    }

    [RelayCommand]
    private void Back()
    {
        ResetIdle();
        if (_backStack.Count > 0)
        {
            FillServices(_backStack.Pop());
            ListTitle = "Выберите услугу";
        }
        else
        {
            GoWelcome();
        }
    }

    // ─── Ввод данных ────────────────────────────────────────

    [RelayCommand]
    private async Task ConfirmFacets()
    {
        if (_facetsService is null) return;
        ResetIdle();
        await CreateTicketAsync(
            _facetsService.Id,
            _facetsService.Letter,
            string.IsNullOrWhiteSpace(PhoneNumber) ? null : PhoneNumber,
            string.IsNullOrWhiteSpace(FullName) ? null : FullName);
    }

    // ─── Создание талона + экран результата ─────────────────

    private async Task CreateTicketAsync(Guid serviceId, string? letter, string? phone, string? name)
    {
        IsBusy = true;
        Error = null;
        try
        {
            var ticket = await _api.CreateTicketAsync(new CreateTicketRequest
            {
                ServiceId = serviceId,
                Letter = letter,
                PhoneNumber = phone,
                FullName = name
            });

            TicketNumber = ticket.Number;
            Step = KioskStep.Ticket;
            StartTicketTimer();
        }
        catch (Exception ex)
        {
            Error = "Не удалось выдать талон: " + ex.Message;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void Done() => GoWelcome();

    [RelayCommand]
    private void Cancel() => GoWelcome();

    private void GoWelcome()
    {
        StopTimers();
        Services.Clear();
        _backStack.Clear();
        IsSimpleMode = false;
        Step = KioskStep.Welcome;
    }

    private void FillServices(IEnumerable<ServiceModel> items)
    {
        Services.Clear();
        foreach (var s in items)
            if (s.IsActive) Services.Add(s); // на терминале показываем только активные
    }

    // ─── Таймеры ────────────────────────────────────────────

    private void StartIdleTimer()
    {
        ResetIdle();
    }

    private void ResetIdle()
    {
        _idleTimer?.Stop();
        _idleTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(_options.IdleTimeoutSeconds) };
        _idleTimer.Tick += (_, _) => GoWelcome();
        _idleTimer.Start();
    }

    private void StartTicketTimer()
    {
        _idleTimer?.Stop();
        _ticketTimer?.Stop();
        SecondsLeft = _options.TicketDisplaySeconds;
        _ticketTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _ticketTimer.Tick += (_, _) =>
        {
            SecondsLeft--;
            if (SecondsLeft <= 0) GoWelcome();
        };
        _ticketTimer.Start();
    }

    private void StopTimers()
    {
        _idleTimer?.Stop();
        _ticketTimer?.Stop();
    }
}
