using Queue.Domain.Entities;

namespace Queue.Applications.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByTitleAsync(string title);
}
