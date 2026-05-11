using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByTitleAsync(string title);
}
