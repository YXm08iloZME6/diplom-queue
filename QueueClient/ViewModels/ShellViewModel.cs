using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QueueClient.Services;
using QueueClient.ViewModels.Admin;

namespace QueueClient.ViewModels;

public record NavItem(string Key, string Title, string Icon);

/// <summary>
/// Оболочка для оператора/админа: слева меню (по роли), справа активный раздел.
/// </summary>
public partial class ShellViewModel : ViewModelBase
{
    private readonly SessionService _session;
    private readonly IViewModelFactory _factory;

    [ObservableProperty] private ViewModelBase? _currentSection;
    [ObservableProperty] private NavItem? _selected;

    public ObservableCollection<NavItem> Items { get; } = new();

    public string UserEmail => _session.Email ?? "";
    public string RoleLabel => _session.IsAdmin ? "Администратор" : "Оператор";

    /// <summary>Поднимается при нажатии «Выйти» — корневой VM возвращает экран входа.</summary>
    public event System.Action? LogoutRequested;

    public ShellViewModel(SessionService session, IViewModelFactory factory)
    {
        _session = session;
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        // Рабочее место — для оператора и для админа
        if (_session.IsOperator || _session.IsAdmin)
            Items.Add(new NavItem("operator", "Рабочее место", "🖥️"));

        // Табло удобно открыть с любого рабочего компьютера
        Items.Add(new NavItem("display", "Табло", "📺"));

        // Разделы администратора
        if (_session.IsAdmin)
        {
            Items.Add(new NavItem("users", "Сотрудники", "👥"));
            Items.Add(new NavItem("services", "Услуги", "🛠"));
            Items.Add(new NavItem("windows", "Окна", "🪟"));
            Items.Add(new NavItem("settings", "Настройки", "⚙️"));
            Items.Add(new NavItem("statistics", "Статистика", "📊"));
        }

        if (Items.Count > 0)
            await SelectAsync(Items[0]);
    }

    [RelayCommand]
    private async Task SelectAsync(NavItem item)
    {
        Selected = item;

        // Освобождаем предыдущий раздел (отписка от SignalR и т.п.)
        if (CurrentSection is System.IDisposable disposable)
            disposable.Dispose();

        CurrentSection = item.Key switch
        {
            "operator"   => _factory.Create<OperatorViewModel>(),
            "display"    => _factory.Create<DisplayViewModel>(),
            "users"      => _factory.Create<AdminUsersViewModel>(),
            "services"   => _factory.Create<AdminServicesViewModel>(),
            "windows"    => _factory.Create<AdminWindowsViewModel>(),
            "settings"   => _factory.Create<AdminSettingsViewModel>(),
            "statistics" => _factory.Create<AdminStatisticsViewModel>(),
            _            => CurrentSection
        };

        // Если у раздела есть начальная загрузка — запускаем
        if (CurrentSection is ILoadable loadable)
            await loadable.LoadAsync();
    }

    [RelayCommand]
    private void Logout()
    {
        // Освобождаем активный раздел (отписка от SignalR)
        if (CurrentSection is System.IDisposable disposable)
            disposable.Dispose();

        _session.Clear();
        LogoutRequested?.Invoke();
    }
}

/// <summary>Раздел, который умеет загружать свои данные при открытии.</summary>
public interface ILoadable
{
    Task LoadAsync();
}
