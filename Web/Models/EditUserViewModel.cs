using Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class EditUserViewModel
{
    public EditUserDto UserData { get; set; }

    [ValidateNever]
    public List<WindowDto>? AvailableWindows { get; set; }
    public List<string> Roles { get; set; } = new();
}
