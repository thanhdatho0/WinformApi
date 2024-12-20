using api.DTOs.Employee;
using api.Models;

namespace api.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> CreateAsync(Employee employee);
    Task<Employee?> GetByCodeAsync(string id);
    Task<Employee?> UpdateAsync(int id, string baseUrl, IFormFile? file, EmployeeUpdateDto employeeUpdateDto);
    // Task<Employee?> DeleteAsync(int id);
}