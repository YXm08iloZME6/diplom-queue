using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Queue.Applications.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AdminService(
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new InvalidOperationException("Такого пользователя не существует");
            }

            return MapToUserDto(user);
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(MapToUserDto).ToList();
        }

        public async Task<UserDto> AddUser(CreateUserDto dto, List<string> roleNames)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException(
                    "Email уже используется");
            }

            var user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                MiddleName = dto.MiddleName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                WindowId = dto.WindowId,
                Status = dto.Status
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            foreach (var roleName in roleNames)
            {
                var role = await _roleRepository.GetByTitleAsync(roleName);

                if (role != null)
                {
                    await _userRepository.AddUserRoleAsync(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                }
            }

            await _userRepository.SaveChangesAsync();

            var createdUser = await _userRepository.GetByIdAsync(user.Id);

            if (createdUser == null)
            {
                throw new InvalidOperationException("Ошибка создания пользователя");
            }

            return MapToUserDto(createdUser);
        }

        public async Task<UserDto> EditUser(EditUserDto dto, List<string> roleNames)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id);

            if (user == null)
            {
                throw new InvalidOperationException("Такого пользователя не существует");
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.MiddleName = dto.MiddleName;
            user.Email = dto.Email;
            user.WindowId = dto.WindowId;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            await _userRepository.RemoveUserRolesAsync(user.Id);

            foreach (var roleName in roleNames)
            {
                var role = await _roleRepository.GetByTitleAsync(roleName);

                if (role != null)
                {
                    await _userRepository.AddUserRoleAsync(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                }
            }

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var updatedUser = await _userRepository.GetByIdAsync(user.Id);

            if (updatedUser == null)
            {
                throw new InvalidOperationException("Ошибка обновления пользователя");
            }

            return MapToUserDto(updatedUser);
        }

        public async Task<bool> RemoveUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _userRepository.RemoveUserRolesAsync(id);
            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                MiddleName = user.MiddleName,
                Status = user.Status,
                Email = user.Email,
                WindowId = user.WindowId,
                Roles = user.UserRoles
                    .Select(ur => ur.Role.Title)
                    .ToList()
            };
        }
    }
}