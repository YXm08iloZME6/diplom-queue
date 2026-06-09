using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;

namespace Queue.Applications.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IWindowRepository _windowRepository;
        private readonly IFileStorageService _fileStorageService;

        public AdminService(IUserRepository userRepository, IRoleRepository roleRepository, 
            IServiceRepository serviceRepository, ITicketRepository ticketRepository, ISettingsRepository settingsRepository, IWindowRepository windowRepository,
            IFileStorageService fileStorageService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _serviceRepository = serviceRepository;
            _ticketRepository = ticketRepository;
            _settingsRepository = settingsRepository;
            _windowRepository = windowRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new InvalidOperationException("Такого пользователя не существует");
            }

            return new UserDto(user);
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserDto(u)).ToList();
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
                Status = dto.Status,
                PhotoPath = await _fileStorageService.SaveFileAsync(dto.Photo, "avatars")
            };

            await _userRepository.AddAsync(user);

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

            var createdUser = await _userRepository.GetByIdAsync(user.Id);

            if (createdUser == null)
            {
                throw new InvalidOperationException("Ошибка создания пользователя");
            }

            return new UserDto(createdUser);
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
            user.Status = dto.Status;

            if (dto.Photo != null)
            {
                if (user.PhotoPath != null)
                {
                    await _fileStorageService.DeleteFileAsync(user.PhotoPath);
                }
                user.PhotoPath = await _fileStorageService.SaveFileAsync(dto.Photo, "avatars");
            }

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

            var updatedUser = await _userRepository.GetByIdAsync(user.Id);

            if (updatedUser == null)
            {
                throw new InvalidOperationException("Ошибка обновления пользователя");
            }

            return new UserDto(updatedUser);
        }

        public async Task<bool> RemoveUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _userRepository.RemoveUserRolesAsync(id);
            await _userRepository.DeleteAsync(user);

            return true;
        }

        public async Task<ServiceDto> AddServiceAsync(CreateServiceDto serviceDto)
        {
            var newService = new Service
            {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                IconName = serviceDto.IconName,
                ParentId = serviceDto.ParentId,
                Letter = serviceDto.ParentId == null ? serviceDto.Letter : null
            };

            await _serviceRepository.CreateServiceAsync(newService);

            return new ServiceDto
            {
                Name = serviceDto.Name,
                Letter = serviceDto.Letter,
                Description = serviceDto.Description,
                IconName = serviceDto.IconName,
                ParentId = serviceDto.ParentId
            };
        }

        public async Task UpdateServiceAsync(UpdateServiceDto dto)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(dto.Id);

            if (service == null)
                throw new Exception("Услуга не найдена");

            service.Name = dto.Name;
            service.Description = dto.Description;
            service.IconName = dto.IconName;

            if (service.ParentId == null)
            {
                service.Letter = dto.Letter;
            }

            await _serviceRepository.UpdateServiceAsync(service);
        }

        public async Task DeleteServiceAsync(Guid id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);

            if (service == null)
                throw new Exception("Услуга не найдена");

            await _serviceRepository.DeleteServiceAsync(service);
        }

        public async Task ToggleServiceStatus(Guid serviceId)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(serviceId);

            if (service == null)
            {
                throw new InvalidOperationException("Такой услуги не существует");
            }

            service.IsActive = !service.IsActive;

            await _serviceRepository.SaveChangeAsync();
        }

        public async Task ToggleServiceFacets(Guid serviceId)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(serviceId);

            if (service == null)
            {
                throw new InvalidOperationException("Такой услуги не существует");
            }

            service.IsNeedFacets = !service.IsNeedFacets;
            await _serviceRepository.SaveChangeAsync();
        }

        public async Task QueueResetAsync()
        {
            var tickets = await _ticketRepository.GetAllActiveAsync();

            foreach (var ticket in tickets)
            {
                ticket.Status = TicketStatus.Cancelled;
                ticket.CompletedAt = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket);
            }

        }

        public async Task<List<TicketDto>> TicketStats(DateTime start, DateTime end, string? status = null, Guid? serviceId = null)
        {
            var tickets = await _ticketRepository.GetFilteredTicketsAsync(start, end, status, serviceId);
            var offset = await GetUtcOffset();
            if (tickets == null) { return new List<TicketDto>(); }
            return tickets.Select(t => new TicketDto(t, offset)).ToList();
        }

        private async Task<int> GetUtcOffset()
        {
            var setting = await _settingsRepository.GetSettingByNameAsync("Часовой пояс");
            if (setting == null || !int.TryParse(setting.Value, out int offset))
            {
                return 0;
            }
            return offset;
        }
        
        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            var todayStart = DateTime.UtcNow.Date;
            var todayEnd = todayStart.AddDays(1);

            var tickets = await _ticketRepository.GetAllAsync();
            var todayTickets = tickets.Where(t => t.CreatedAt >= todayStart && t.CreatedAt < todayEnd).ToList();
            var completedToday = todayTickets.Count(t => t.Status == TicketStatus.Completed);
            var waitingNow = tickets.Count(t => t.Status == TicketStatus.Waiting);

            var operators = await _userRepository.GetAllAsync();

            var services = await _serviceRepository.GetMainServicesAsync();
            var serviceStats = services.Select(s => new ServiceStatDto
            {
                ServiceName = s.Name,
                TicketCount = tickets.Count(t => t.ServiceId == s.Id)
            }).OrderByDescending(x => x.TicketCount).ToList();

            var lastTickets = tickets.OrderByDescending(t => t.CreatedAt).Take(10).Select(t => new TicketDto(t)).ToList();

            var windows = await _windowRepository.GetAllWindowsAsync();
            var windowsList = windows.OrderBy(t => t.Number).Select(w => new WindowDto(w)).ToList();

            return new AdminDashboardDto
            {
                TicketsToday = todayTickets.Count,
                CompletedToday = completedToday,
                WaitingNow = waitingNow,
                ActiveOperators = operators.Count,
                ServiceStats = serviceStats,
                LastTickets = lastTickets,
                Windows = windowsList
            };
        }
    }
}