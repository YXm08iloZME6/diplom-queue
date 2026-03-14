namespace Queue.Models;
using System.ComponentModel.DataAnnotations;

public class Client
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Info { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}