using Domain.Enums;

namespace Application.DTOs;

public class UsersDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Status { get; set; }
    public string Email { get; set; }
    public Guid ServiceId { get; set; }
    public List<string> Roles { get; set; }
}

public class CreateUserDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid ServiceId { get; set; }
}

public class LoginUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid ServiceId { get; set; }
}

public class UpdateUserStatusDto
{
    public Guid UserId { get; set; }
    public UserStatus Status { get; set; }
}