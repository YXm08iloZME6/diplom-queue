namespace Queue.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}