using Application.DTOs;
using Queue.Domain.Entities;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<bool> ValidateUser(LoginUserDto user);
    Task<UserDto> GetUserByEmail(string email);

    Task<UserDto> CreateUser(RegisterUserDto user);

}
