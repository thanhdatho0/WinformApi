
using api.DTOs.TargetCustomer;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/targetCustomers")]
    public class TargetCustomerController(ITargetCustomerRepository targetCustomerRepo) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TargetCustomerQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var targets = await targetCustomerRepo.GetAllAsync(query);

            var targerDto = targets.Select(g => g.ToTargetCustomerDto());

            return Ok(targerDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var target = await targetCustomerRepo.GetByIdAsync(id);

            if (target == null)
                return NotFound();

            return Ok(target.ToTargetCustomerDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TargetCustomerCreateDto targetCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await targetCustomerRepo.TargetCustomerNameExists(targetCustomerDto.TargetCustomerName))
                return BadRequest("Target customer name already exists");

            var targetCustomer = targetCustomerDto.ToTargetCustomerFromCreateDto();
            await targetCustomerRepo.CreateAsync(targetCustomer);

            return CreatedAtAction(nameof(GetById), new { id = targetCustomer.TargetCustomerId }, targetCustomer.ToTargetCustomerDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TargetCustomerUpdateDto targetCustomerUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await targetCustomerRepo.TargetCustomerNameExists(targetCustomerUpdateDto.TargetCustomerName))
                return BadRequest("Target customer name already exists");

            var targetCustomer = await targetCustomerRepo.UpdateAsync(id, targetCustomerUpdateDto);

            return Ok(targetCustomer?.ToTargetCustomerDto());
        }

    }
}