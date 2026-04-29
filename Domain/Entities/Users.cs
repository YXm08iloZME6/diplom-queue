using Domain.Entities;
using Domain.Enums;


namespace Queue.Domain.Entities;

public class Users
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public UserStatus Status { get; set; } 
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }
    public ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
}