using Domain.Entities;

namespace Queue.Applications.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);

    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task RemoveUserRolesAsync(Guid userId);
    Task AddUserRoleAsync(UserRole userRole);
    Task SaveChangesAsync();

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}