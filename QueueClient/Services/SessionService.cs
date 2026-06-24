using System;
using System.Collections.Generic;
using System.Linq;

namespace QueueClient.Services;

/// <summary>Хранит данные текущего сеанса: JWT, id пользователя, роли.</summary>
public class SessionService
{
    public string? Token { get; private set; }
    public Guid UserId { get; private set; }
    public string? Email { get; private set; }
    public IReadOnlyList<string> Roles { get; private set; } = Array.Empty<string>();

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    public bool IsAdmin => Roles.Contains("admin", StringComparer.OrdinalIgnoreCase);
    public bool IsOperator => Roles.Contains("operator", StringComparer.OrdinalIgnoreCase);

    /// <summary>Событие на случай, если токен сбросили (разлогин/протух).</summary>
    public event Action? Changed;

    public void Set(string token, Guid userId, string? email, IEnumerable<string>? roles)
    {
        Token = token;
        UserId = userId;
        Email = email;
        Roles = roles?.ToList() ?? new List<string>();
        Changed?.Invoke();
    }

    public void Clear()
    {
        Token = null;
        UserId = Guid.Empty;
        Email = null;
        Roles = Array.Empty<string>();
        Changed?.Invoke();
    }
}
