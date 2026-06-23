using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/audit")]
[Authorize(Roles = "admin")]
public class AuditController : ControllerBase
{
    private readonly IAuditService _service;

    public AuditController(IAuditService service) => _service = service;

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers() =>
        Ok(await _service.GetAllUsersAsync());

    [HttpGet("users/{userId:int}/employees")]
    public async Task<IActionResult> GetEmployees(int userId) =>
        Ok(await _service.GetEmployeesByUserAsync(userId));

    [HttpGet("users/{userId:int}/shifts")]
    public async Task<IActionResult> GetShifts(int userId) =>
        Ok(await _service.GetShiftsByUserAsync(userId));

    [HttpGet("users/{userId:int}/plans")]
    public async Task<IActionResult> GetPlans(int userId) =>
        Ok(await _service.GetPlansByUserAsync(userId));

    [HttpGet("users/{userId:int}/schedules")]
    public async Task<IActionResult> GetSchedules(
        int userId,
        [FromQuery] DateOnly? start,
        [FromQuery] DateOnly? end) =>
        Ok(await _service.GetSchedulesByUserAsync(userId, start, end));

    [HttpGet("users/{userId:int}/vacations")]
    public async Task<IActionResult> GetVacations(int userId) =>
        Ok(await _service.GetVacationsByUserAsync(userId));

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats() =>
        Ok(await _service.GetGlobalStatsAsync());
}
