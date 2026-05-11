using Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class CreateWindowViewModel
{
    public CreateWindowDto Window { get; set; }
    
    [ValidateNever]
    public List<ServiceDto> Services { get; set; }
}
