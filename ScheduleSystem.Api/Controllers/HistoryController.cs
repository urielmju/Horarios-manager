using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/history")]
[Authorize]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _service;

    public HistoryController(IHistoryService service) => _service = service;

    private int OwnerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var (items, total) = await _service.GetPagedAsync(OwnerId, page, pageSize);
        return Ok(new { items, total, page, pageSize });
    }
}
