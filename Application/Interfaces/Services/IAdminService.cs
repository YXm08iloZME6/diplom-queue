using Application.DTOs;

namespace Application.Interfaces.Services;
    public interface IAdminService
{
    Task<UserDto> GetUserById(Guid id);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto> AddUser(CreateUserDto dto,List<string> roleNames);
    Task<UserDto> EditUser(EditUserDto dto,List<string> roleNames);
    Task<bool> RemoveUser(Guid id);
    Task ToggleServiceStatus(Guid serviceId);
}