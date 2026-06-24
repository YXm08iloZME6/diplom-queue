using System;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueClient.Configuration;
using QueueClient.Services;
using QueueClient.ViewModels;
using QueueClient.ViewModels.Admin;
using QueueClient.Views;

namespace QueueClient;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var provider = ConfigureServices().BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainVm = provider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow { DataContext = mainVm };

            // Старт логики после показа окна (вход/режим) — не блокируем UI-поток
            Dispatcher.UIThread.Post(async () => await mainVm.InitializeAsync());
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var options = config.GetSection(AppOptions.SectionName).Get<AppOptions>() ?? new AppOptions();
        services.AddSingleton(options);

        services.AddSingleton(_ =>
        {
            var handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#endif
            return new HttpClient(handler)
            {
                BaseAddress = new Uri(options.ApiBaseUrl.TrimEnd('/') + "/")
            };
        });

        // Сервисы
        services.AddSingleton<SessionService>();
        services.AddSingleton<IApiClient, ApiClient>();
        services.AddSingleton<SignalRService>();
        services.AddSingleton<IViewModelFactory, ViewModelFactory>();

        // Корневой и постоянные VM
        services.AddSingleton<MainWindowViewModel>();

        // Экраны — Transient (свежие при каждом переходе)
        services.AddTransient<LoginViewModel>();
        services.AddTransient<ShellViewModel>();
        services.AddTransient<OperatorViewModel>();
        services.AddTransient<DisplayViewModel>();
        services.AddTransient<KioskViewModel>();
        services.AddTransient<AdminUsersViewModel>();
        services.AddTransient<AdminServicesViewModel>();
        services.AddTransient<AdminWindowsViewModel>();
        services.AddTransient<AdminSettingsViewModel>();
        services.AddTransient<AdminStatisticsViewModel>();

        return services;
    }
}
