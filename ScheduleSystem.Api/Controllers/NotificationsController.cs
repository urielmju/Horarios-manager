using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service) => _service = service;

    private int OwnerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync(OwnerId));

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount() =>
        Ok(new { count = await _service.GetUnreadCountAsync(OwnerId) });

    [HttpPatch("mark-all-read")]
    public async Task<IActionResult> MarkAllRead()
    {
        await _service.MarkAllReadAsync(OwnerId);
        return Ok(new { message = "All notifications marked as read." });
    }
}
