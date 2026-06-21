using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Models.DTOs.Captain;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/captains")]
[Authorize]
public class CaptainsController : ControllerBase
{
    private readonly ICaptainService _service;

    public CaptainsController(ICaptainService service) => _service = service;

    private int OwnerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetByDateAndPlan([FromQuery] DateOnly date, [FromQuery] int planId)
    {
        var result = await _service.GetByDateAndPlanAsync(OwnerId, date, planId);
        if (result is null) return NotFound(new { message = "No captain assigned." });
        return Ok(result);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignCaptainDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.AssignAsync(dto, OwnerId);
        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount([FromQuery] int employeeId, [FromQuery] int planId)
    {
        var count = await _service.GetCountByEmployeeAndPlanAsync(OwnerId, employeeId, planId);
        return Ok(new { count });
    }
}
