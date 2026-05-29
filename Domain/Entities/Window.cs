using Domain.Entities;
using Domain.Enums;


namespace Domain.Entities;

public class Window
{
    public Guid Id { get; set; }
    public string? Number { get; set; }
    public WindowStatus Status { get; set; }

    public Guid? ServiceId { get; set; }
    public Service? Service { get; set; }
    public ICollection<User> Operators { get; set; } = new List<User>();
}