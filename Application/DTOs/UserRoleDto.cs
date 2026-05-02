namespace Application.DTOs;

public class UserRolesDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public Guid RoleId { get; set; }
    public string RoleTitle { get; set; }
}

public class AddRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

public class RemoveRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}