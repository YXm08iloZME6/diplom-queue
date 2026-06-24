namespace QueueClient.Configuration;

public enum AppMode
{
    /// <summary>Обычный компьютер: экран входа, дальше по роли.</summary>
    Login,
    /// <summary>Сенсорный терминал выдачи талонов.</summary>
    Kiosk,
    /// <summary>Табло в зале ожидания.</summary>
    Display
}

/// <summary>Настройки клиента (секция "App" в appsettings.json).</summary>
public class AppOptions
{
    public const string SectionName = "App";

    public string ApiBaseUrl { get; set; } = "https://localhost:7148";
    public AppMode Mode { get; set; } = AppMode.Login;

    /// <summary>Учётка для автоматического входа в режимах Kiosk/Display.</summary>
    public string ServiceAccountLogin { get; set; } = "";
    public string ServiceAccountPassword { get; set; } = "";

    public int IdleTimeoutSeconds { get; set; } = 60;
    public int TicketDisplaySeconds { get; set; } = 10;
}
