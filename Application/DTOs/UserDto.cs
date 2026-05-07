using Domain.Enums;

namespace Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Status { get; set; }
    public string Email { get; set; }
    public Guid? WindowId { get; set; }
    public List<string> Roles { get; set; }
}

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class CreateUserDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
    public Guid? WindowId { get; set; }
}

public class LoginUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class EditUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
    public Guid? WindowId { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class UpdateUserStatusDto
{
    public Guid UserId { get; set; }
    public UserStatus Status { get; set; }
}

