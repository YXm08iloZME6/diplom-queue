using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using QueueClient.Configuration;
using QueueClient.Models;
using QueueClient.Services;

namespace QueueClient.Services;

/// <summary>
/// Подключение к SignalR-хабу /queueHub. Сервер шлёт события NewTicket / UpdateTicket
/// (полезная нагрузка — TicketDto). Клиент по ним обновляет данные.
/// События пробрасываются в UI-поток.
/// </summary>
public class SignalRService : IAsyncDisposable
{
    private readonly AppOptions _options;
    private readonly SessionService _session;
    private HubConnection? _connection;

    public event Action<TicketModel>? NewTicket;
    public event Action<TicketModel>? UpdateTicket;

    public SignalRService(AppOptions options, SessionService session)
    {
        _options = options;
        _session = session;
    }

    public async Task StartAsync()
    {
        if (_connection is not null) return;

        var hubUrl = _options.ApiBaseUrl.TrimEnd('/') + "/queueHub";

        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl, opts =>
            {
                // Хаб может требовать авторизацию — передаём тот же JWT
                opts.AccessTokenProvider = () => Task.FromResult(_session.Token);
#if DEBUG
                // Обход самоподписанного сертификата на localhost (только в DEBUG)
                opts.HttpMessageHandlerFactory = _ => new System.Net.Http.HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        System.Net.Http.HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
#endif
            })
            .WithAutomaticReconnect()
            .Build();

        _connection.On<TicketModel>("NewTicket", t =>
            Dispatcher.UIThread.Post(() => NewTicket?.Invoke(t)));

        _connection.On<TicketModel>("UpdateTicket", t =>
            Dispatcher.UIThread.Post(() => UpdateTicket?.Invoke(t)));

        try
        {
            await _connection.StartAsync();
        }
        catch
        {
            // Не валим приложение, если хаб недоступен — экраны просто не будут
            // обновляться вживую (можно обновить вручную).
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
