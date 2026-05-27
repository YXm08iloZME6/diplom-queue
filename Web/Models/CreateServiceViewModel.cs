using Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class CreateServiceViewModel
{
    public CreateServiceDto Service { get; set; }

    [ValidateNever]
    public List<ServiceDto>? AvailableParents { get; set; }
}
