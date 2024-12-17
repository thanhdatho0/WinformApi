using System.Security.Claims;
using api.DTOs.Account;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    SignInManager<AppUser> signInManager,
    RoleManager<IdentityRole> roleManager,
    IEmployeeRepository employeeRepository,
    ICustomerRepository customerRepository)
    : ControllerBase
{
    [HttpPost("admin-register")]
    public async Task<ActionResult> RegisterAdmin([FromBody] AdminRegisterDto adminRegisterDto)
    {
        try
        {
            // Validate the incoming model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if "Admin" role exists, and create it if not
            var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return StatusCode(500, $"Failed to create Admin role: {roleErrors}");
                }
            }

            // Create a new user instance
            var appUser = new AppUser
            {
                UserName = adminRegisterDto.Username,
                Email = adminRegisterDto.Email
            };

            // Attempt to create the user
            var createUser = await userManager.CreateAsync(appUser, adminRegisterDto.Password);
            if (!createUser.Succeeded)
            {
                var userErrors = string.Join(", ", createUser.Errors.Select(e => e.Description));
                return BadRequest($"Failed to create user: {userErrors}");
            }

            // Assign the user to the "Admin" role
            var roleAssignment = await userManager.AddToRoleAsync(appUser, "Admin");
            if (!roleAssignment.Succeeded)
            {
                var roleAssignmentErrors = string.Join(", ", roleAssignment.Errors.Select(e => e.Description));
                return BadRequest($"Failed to add user to Admin role: {roleAssignmentErrors}");
            }

            await userManager.AddClaimAsync(appUser, new Claim("Role", "Admin"));
            var role = await roleManager.FindByNameAsync("Admin");
            if (role != null)
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", "AdminAccess"));
            }

            return Ok("Admin Created Successfully");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }

    [HttpPost("customer-register")]
    public async Task<ActionResult> Register([FromBody] CustomerRegisterDto customerRegisterDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var adminRoleExists = await roleManager.RoleExistsAsync("Customer");
            if (!adminRoleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole("Customer"));
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return StatusCode(500, $"Failed to create Customer role: {roleErrors}");
                }
            }

            var appUser = new AppUser
            {
                UserName = customerRegisterDto.Username,
                Email = customerRegisterDto.CustomerInfo.Email,
                PhoneNumber = customerRegisterDto.CustomerInfo.PersonalInfo.PhoneNumber,
            };

            var createUser = await userManager.CreateAsync(appUser, customerRegisterDto.Password);
            // Attempt to create the user
            if (!createUser.Succeeded)
            {
                var userErrors = string.Join(", ", createUser.Errors.Select(e => e.Description));
                return BadRequest($"Failed to create user: {userErrors}");
            }

            var roleAssignment = await userManager.AddToRoleAsync(appUser, "Customer");
            if (!roleAssignment.Succeeded)
            {
                var roleAssignmentErrors = string.Join(", ", roleAssignment.Errors.Select(e => e.Description));
                return BadRequest($"Failed to add user to Admin role: {roleAssignmentErrors}");
            }
            await userManager.AddClaimAsync(appUser, new Claim("Role", "Customer"));
            var role = await roleManager.FindByNameAsync("Customer");
            if (role != null)
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", "CustomerAccess"));
            }
            
            var newCustomer = customerRegisterDto.CustomerInfo.ToCustomerCreateDto();
            newCustomer.CustomerCode = appUser.Id;
            newCustomer.Avatar = $"{Request.Scheme}://{Request.Host}/images/customer-avatar.png";
            await customerRepository.CreateAsync(newCustomer);
            
            return Ok("Customer Created");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occured: {e.Message}");
        }
    }

    [HttpPost("employee-register")]
    public async Task<ActionResult> RegisterEmployee([FromBody] EmployeeRegisterDto employeeRegisterDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Check if "Employee" role exists, and create it if not
            var employeeRoleExists = await roleManager.RoleExistsAsync("Employee");
            if (!employeeRoleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole("Employee"));
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return StatusCode(500, $"Failed to create Employee role: {roleErrors}");
                }
            }

            var appUser = new AppUser
            {
                UserName = employeeRegisterDto.Username,
                Email = employeeRegisterDto.Email,
                PhoneNumber = employeeRegisterDto.EmployeeInfo.PersonalInfo.PhoneNumber
            };

            // Attempt to create the user
            var createUser = await userManager.CreateAsync(appUser, employeeRegisterDto.Password);
            if (!createUser.Succeeded)
            {
                var userErrors = string.Join(", ", createUser.Errors.Select(e => e.Description));
                return BadRequest($"Failed to create user: {userErrors}");
            }

            // Assign the user to the "Admin" role
            var roleAssignment = await userManager.AddToRoleAsync(appUser, "Employee");
            if (!roleAssignment.Succeeded)
            {
                var roleAssignmentErrors = string.Join(", ", roleAssignment.Errors.Select(e => e.Description));
                return BadRequest($"Failed to add user to Employee role: {roleAssignmentErrors}");
            }

            await userManager.AddClaimAsync(appUser, new Claim("Role", "Employee"));
            var role = await roleManager.FindByNameAsync("Employee");
            if (role != null)
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", "EmployeeAccess"));
            }

            var employeeModel = employeeRegisterDto.EmployeeInfo.ToCreateEmployeeDto();
            employeeModel.EmployeeCode = appUser.Id;
            await employeeRepository.CreateAsync(employeeModel);

            return Ok("Employee Created Successfully");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username);

        if (user == null
            || !(await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false)).Succeeded
         )
            return Unauthorized("Username or password is incorrect!");

        var tokenDto = await tokenService.CreateToken(user, true);
        return Ok(tokenDto.AccessToken);
    }
    
    [HttpPost("admin/login")]
    public async Task<ActionResult> AdminLogin([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username);

        if (user == null
            || !(await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false)).Succeeded
           )
            return Unauthorized("Username or password is incorrect!");

        var tokenDto = await tokenService.CreateToken(user, true);
        return Ok(tokenDto.AccessToken);
    }
    
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] NewPasswordDto changePasswordDto)
    {
        var user = await userManager.FindByNameAsync(changePasswordDto.UserName);
        if (user == null) return Unauthorized("Invalid username!");
        if (!await userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword))
            return Unauthorized("Incorrect password!");
        if (!changePasswordDto.NewPassword.Equals(changePasswordDto.ConfirmNewPassword))
            return BadRequest("Passwords don't match!");
        var validationPassword = await userManager.PasswordValidators[0].ValidateAsync(userManager, user, changePasswordDto.NewPassword);
        if (!validationPassword.Succeeded)
        {
            return BadRequest(validationPassword.Errors);
        }
        try
        {
            await userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            return Ok("Password changed successfully!");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An error occured: {e.Message}");
        }
    }
}