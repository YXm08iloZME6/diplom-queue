using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Queue.Applications.Interfaces;
using Domain.Entities;

namespace Queue.Applications.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto?> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                MiddleName = user.MiddleName,
                Status = user.Status.ToString(),
                Email = user.Email,
                WindowId = user.WindowId,
                Roles = user.UserRoles
                    .Select(ur => ur.Role.Title)
                    .ToList()
            };
        }

        public async Task<bool> ValidateUser(LoginUserDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        }

        public async Task<UserDto> CreateUser(RegisterUserDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new InvalidOperationException("Пароли не совпадают");

            if (await _userRepository.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("Email уже используется");

            await _userRepository.BeginTransactionAsync();

            try
            {
                var user = new User
                {
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Status = UserStatus.Waiting
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                var role = await _roleRepository.GetByTitleAsync("operator");

                if (role == null)
                    throw new InvalidOperationException("Роль 'operator' не существует");

                await _userRepository.AddUserRoleAsync(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });

                await _userRepository.SaveChangesAsync();
                await _userRepository.CommitTransactionAsync();

                var created = await _userRepository.GetByIdAsync(user.Id);

                return new UserDto
                {
                    Id = created.Id,
                    Email = created.Email,
                    Status = created.Status.ToString(),
                    Roles = created.UserRoles.Select(r => r.Role.Title).ToList()
                };
            }
            catch
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}