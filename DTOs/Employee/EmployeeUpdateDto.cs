using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Employee;

public class EmployeeUpdateDto
{
    public EmployeePersonalInfo PersonalInfo { get; set; }
    [DefaultValue("")]
    public string Email { get; set; } = string.Empty;
}