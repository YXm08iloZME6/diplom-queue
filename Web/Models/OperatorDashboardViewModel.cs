using Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class OperatorDashboardViewModel
{
    public OperatorDashboardDto dashboard {  get; set; }
    public string? serviceName { get; set; }
    public string? windowName { get; set; }

}
