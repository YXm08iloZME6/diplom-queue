
using Application.Interfaces;
using Infrastructure.Data;
using Queue.Domain.Entities;

namespace Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly QueueDbContext _context;

    public UserRepository(QueueDbContext context)
    {
        _context = context;
    }

    public Task AddUserRoleAsync(UserRoles userRole)
    {
        throw new NotImplementedException();
    }

    public Task<Users> CreateUserAsync(Users user)
    {
        throw new NotImplementedException();
    }

    public Task<Roles?> GetRoleByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }

    public Task<Users?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailExistsAsync(string email)
    {
        throw new NotImplementedException();
    }
}


