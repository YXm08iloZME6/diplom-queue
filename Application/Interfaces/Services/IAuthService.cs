using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> ValidateUser(LoginUserDto user);
    Task<UserDto> GetUserByEmail(string email);

    Task<UserDto> CreateUser(RegisterUserDto user);

}
