using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Models.DTOs.DaysOff;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/daysoff")]
[Authorize]
public class DaysOffController : ControllerBase
{
    private readonly IDaysOffService _service;

    public DaysOffController(IDaysOffService service) => _service = service;

    private int OwnerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync(OwnerId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id, OwnerId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDaysOffDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.CreateAsync(dto, OwnerId);
        return Created($"api/daysoff/{result.Id}", result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateDaysOffDto dto)
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
