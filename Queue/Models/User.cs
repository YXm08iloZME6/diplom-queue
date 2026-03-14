namespace Queue.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public string? Status { get; set; } 
    public string? Email { get; set; }
    public string? Password { get; set; }

    public Guid? ServiceId { get; set; }
    [ForeignKey("ServiceId")]
    public virtual Service? Service { get; set; }
}