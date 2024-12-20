using api.DTOs.Employee;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(IEmployeeRepository employeeRepository, 
    ITokenService tokenService, 
    UserManager<AppUser> userManager) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var employees = await employeeRepository.GetAllAsync();
        return Ok(employees.Select(x => x.ToEmployeeDto()));
    }

    [HttpGet("details")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetDetails()
    {
        var accessToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        var user = await userManager.FindByNameAsync(principal.Identity!.Name!);
        if(user == null) return Unauthorized();
        var employee = await employeeRepository.GetByCodeAsync(user.Id);
        if (employee == null) return NotFound();
        return Ok(employee.ToEmployeeDto());
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var employee = await employeeRepository.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee.ToEmployeeDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, IFormFile? file, EmployeeUpdateDto employeeUpdateDto)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var employee = await employeeRepository.UpdateAsync(id, baseUrl, file, employeeUpdateDto); 
        return employee != null ? Ok(employee.ToEmployeeDto()) : NotFound();
    }
}