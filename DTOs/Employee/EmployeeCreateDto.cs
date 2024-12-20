using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Employee;

public class EmployeeCreateDto
{
    public EmployeePersonalInfo PersonalInfo { get; set; }
    [Required(ErrorMessage = "Salary is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
}