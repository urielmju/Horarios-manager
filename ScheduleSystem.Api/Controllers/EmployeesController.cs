using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Models.DTOs.Employee;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service) => _service = service;

    private int OwnerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync(OwnerId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) =>
        Ok(await _service.GetByIdAsync(id, OwnerId));

    [HttpPost]
    [Authorize(Roles = "admin,supervisor")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.CreateAsync(dto, OwnerId);
        return Created($"api/employees/{result.Id}", result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,supervisor")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _service.UpdateAsync(id, dto, OwnerId));
    }

    [HttpPatch("{id}/toggle-active")]
    [Authorize(Roles = "admin,supervisor")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        await _service.ToggleActiveAsync(id, OwnerId);
        return Ok(new { message = "Employee status toggled." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id, OwnerId);
        return NoContent();
    }
}
