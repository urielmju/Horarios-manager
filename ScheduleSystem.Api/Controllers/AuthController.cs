using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleSystem.Api.Models.DTOs.Auth;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpGet("has-users")]
    public async Task<IActionResult> HasUsers()
    {
        var has = await _auth.HasUsersAsync();
        return Ok(new { hasUsers = has });
    }

    [HttpPost("setup")]
    public async Task<IActionResult> Setup([FromBody] SetupRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _auth.SetupAsync(dto);
        return Created("", result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _auth.RegisterAsync(dto);
        return Created("", result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _auth.LoginAsync(dto);
        return Ok(result);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _auth.ChangePasswordAsync(userId, dto);
        return Ok(new { message = "Password changed successfully." });
    }
}
