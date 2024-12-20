using api.Data;
using api.DTOs.Employee;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class EmployeeRepository(ApplicationDbContext context, IImageService imageService) : IEmployeeRepository
{
    public async Task<List<Employee>> GetAllAsync()
    {
        return await context.Employees.ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await context.Employees.FindAsync(id);
    }

    public async Task<Employee?> CreateAsync(Employee employee)
    {
        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee?> GetByCodeAsync(string id)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode == id);
        return employee ?? null;
    }

    public async Task<Employee?> UpdateAsync(int id, string baseUrl, IFormFile? file, EmployeeUpdateDto employeeUpdateDto)
    {
        var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == id);
        if (employee == null) return null;
        employee.ToEmployeeUpdate(employeeUpdateDto);
        var user = context.Users.FirstOrDefault(u => u.Id == employee.EmployeeCode);
        if (user == null) return null;
        user.Email = employee.Enail;
        user.NormalizedEmail = employee.Enail.ToUpper();
        user.PhoneNumber = employee.PhoneNumber;
        if (file != null)
        {
            employee.Avatar = await imageService.CreateUrlAsync(file, baseUrl);
        }
        await context.SaveChangesAsync();
        return employee;
    }
}