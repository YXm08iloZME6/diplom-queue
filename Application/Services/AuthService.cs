using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Enums;
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

            return new UserDto(user);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                return null;

            return new UserDto(user);
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

                var createdUser = await _userRepository.GetByIdAsync(user.Id);

                return new UserDto(createdUser);
            }
            catch
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}