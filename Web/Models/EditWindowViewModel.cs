using Application.DTOs;
namespace Web.Models;
public class EditWindowViewModel
{
    public UpdateWindowDto Window { get; set; }
    public List<ServiceDto> Services { get; set; } = new();
}