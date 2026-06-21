using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Models.DTOs.Vacation;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/vacations")]
[Authorize]
public class VacationsController : ControllerBase
{
    private readonly IVacationService _service;

    public VacationsController(IVacationService service) => _service = service;

    private int OwnerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync(OwnerId));

    [HttpGet("active")]
    public async Task<IActionResult> GetActive([FromQuery] DateOnly date) =>
        Ok(await _service.GetActiveOnDateAsync(OwnerId, date));

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(int employeeId) =>
        Ok(await _service.GetByEmployeeAsync(OwnerId, employeeId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id, OwnerId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVacationDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.CreateAsync(dto, OwnerId);
        return Created($"api/vacations/{result.Id}", result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateVacationDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _service.UpdateAsync(id, dto, OwnerId));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id, OwnerId);
        return NoContent();
    }
}
