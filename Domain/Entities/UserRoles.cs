namespace Queue.Domain.Entities;

public class UserRoles
{
    public Guid UserId { get; set; }
    public Users User { get; set; }

    public Guid RoleId { get; set; }
    public Roles Role { get; set; }
}