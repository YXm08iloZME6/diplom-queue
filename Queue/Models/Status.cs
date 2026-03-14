namespace Queue.Models;
using System.ComponentModel.DataAnnotations;

public class Status
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<Window> Windows { get; set; } = new List<Window>();
}