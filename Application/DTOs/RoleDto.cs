namespace Application.DTOs;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }

}

public class CreateRoleDto
{
    public string Title { get; set; }
}

public class UpdateRoleNameDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}
