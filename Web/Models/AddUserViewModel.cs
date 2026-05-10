using Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class AddUserViewModel
{
    public CreateUserDto UserData { get; set; }
    
    [ValidateNever]
    public List<WindowDto>? AvailableWindows { get; set; }
}
