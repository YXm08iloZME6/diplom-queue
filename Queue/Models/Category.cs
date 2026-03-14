namespace Queue.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid? ParentId { get; set; }
    [ForeignKey("ParentId")]
    public virtual Category? Parent { get; set; }
    
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}