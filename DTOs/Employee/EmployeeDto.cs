
using System.ComponentModel;

namespace api.DTOs.Employee;

public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public EmployeePersonalInfo PersonalInfo { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateOnly StartDate { get; set; }
    public int ContractUpTo { get; set; }
    [DefaultValue("")]
    public string? Avatar { get; set; }
}