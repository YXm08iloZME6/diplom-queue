namespace Queue.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid? ClientId { get; set; }
    [ForeignKey("ClientId")]
    public virtual Client? Client { get; set; }

    public DateTime? CreatedAt { get; set; }
    public string? Number { get; set; }
    public TimeSpan? EstimationTime { get; set; }

    [Column(TypeName = "jsonb")]
    public string Facets { get; set; } = "{}";

    public Guid? ServiceId { get; set; }
    [ForeignKey("ServiceId")]
    public virtual Service? Service { get; set; }

    public Guid? StatusId { get; set; }
    [ForeignKey("StatusId")]
    public virtual Status? Status { get; set; }

    public Guid? WindowId { get; set; }
    [ForeignKey("WindowId")]
    public virtual Window? Window { get; set; }   
}