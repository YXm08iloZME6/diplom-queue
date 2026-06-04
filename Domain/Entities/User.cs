using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public UserStatus Status { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? PhotoPath { get; set; }

    public Guid? WindowId { get; set; }
    public Window? Window { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}