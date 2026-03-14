namespace Queue.Models;
using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Title { get; set; }
    
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}