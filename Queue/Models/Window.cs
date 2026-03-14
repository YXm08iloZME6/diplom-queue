namespace Queue.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Window
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int Number { get; set; }

    public Guid? UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public Guid? ServiceId { get; set; }
    [ForeignKey("ServiceId")]
    public virtual Service? Service { get; set; }

    public Guid? StatusId { get; set; }
    [ForeignKey("StatusId")]
    public virtual Status? Status { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}