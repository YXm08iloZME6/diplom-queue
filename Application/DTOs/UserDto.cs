using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public UserStatus Status { get; set; }
    public string Email { get; set; }
    public Guid? WindowId { get; set; }
    public string? WindowName { get; set; }
    public string? PhotoPath { get; set; }
    public List<string> Roles { get; set; }
    public string ServiceName { get; set; }
    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Surname = user.Surname;
        MiddleName = user.MiddleName;
        Status = user.Status;
        Email = user.Email;
        WindowId = user.WindowId;
         WindowName = user.Window?.Number;
        Roles = user.UserRoles.Select(ur => ur.Role.Title).ToList();
        PhotoPath = user.PhotoPath;
        ServiceName = user.Window?.Service?.Name;
    }
}

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class CreateUserDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile? Photo { get; set; }
    public UserStatus Status { get; set; }
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
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public IFormFile? Photo { get; set; }
    public string? PhotoPath { get; set; }
    public UserStatus Status { get; set; }
    public Guid? WindowId { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class UpdateUserStatusDto
{
    public Guid UserId { get; set; }
    public UserStatus Status { get; set; }
}

