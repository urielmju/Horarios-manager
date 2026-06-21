using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Models.DTOs.Schedule;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/schedules")]
[Authorize]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _service;

    public SchedulesController(IScheduleService service) => _service = service;

    private int OwnerId    => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private int UserId     => OwnerId;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] DateOnly? date,
        [FromQuery] DateOnly? start,
        [FromQuery] DateOnly? end,
        [FromQuery] int? employeeId)
    {
        if (date.HasValue)
            return Ok(await _service.GetByDateAsync(OwnerId, date.Value));

        if (start.HasValue && end.HasValue && employeeId.HasValue)
            return Ok(await _service.GetByEmployeeRangeAsync(OwnerId, employeeId.Value, start.Value, end.Value));

        if (start.HasValue && end.HasValue)
            return Ok(await _service.GetByRangeAsync(OwnerId, start.Value, end.Value));

        return BadRequest(new { message = "Provide date, or start+end, or start+end+employeeId." });
    }

    [HttpGet("plan/{planId}")]
    public async Task<IActionResult> GetByPlan(int planId) =>
        Ok(await _service.GetByPlanAsync(OwnerId, planId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScheduleDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.CreateAsync(dto, OwnerId, UserId);
        return Created($"api/schedules/{result.Id}", result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteByRange([FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        await _service.DeleteByRangeAsync(OwnerId, start, end);
        return NoContent();
    }

    [HttpDelete("plan/{planId}")]
    public async Task<IActionResult> DeleteByPlan(int planId)
    {
        await _service.DeleteByPlanAsync(OwnerId, planId);
        return NoContent();
    }
}
