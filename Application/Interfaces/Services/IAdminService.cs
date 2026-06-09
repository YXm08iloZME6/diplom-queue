using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;
    public interface IAdminService
{
    Task<UserDto> GetUserById(Guid id);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto> AddUser(CreateUserDto dto, List<string> roleNames);
    Task<UserDto> EditUser(EditUserDto dto, List<string> roleNames);
    Task<bool> RemoveUser(Guid id);
    Task RemovePhoto(Guid id);
    Task<List<TicketDto>> TicketStats(DateTime start, DateTime end, string? status = null, Guid? serviceId = null);
    Task<ServiceDto> AddServiceAsync(CreateServiceDto serviceDto);
    Task UpdateServiceAsync(UpdateServiceDto serviceDto);
    Task DeleteServiceAsync(Guid serviceId);
    Task ToggleServiceStatus(Guid serviceId);
    Task ToggleServiceFacets(Guid serviceId);
    Task QueueResetAsync();
    Task<AdminDashboardDto> GetDashboardDataAsync();
}