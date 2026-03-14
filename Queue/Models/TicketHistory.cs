namespace Queue.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TicketHistory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid? UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public Guid? TicketId { get; set; }
    [ForeignKey("TicketId")]
    public virtual Ticket? Ticket { get; set; }

    public DateTime? ActionTime { get; set; }
}