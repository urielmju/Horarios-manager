using ScheduleSystem.Api.Models.DTOs.Auth;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<LoginResponseDto> SetupAsync(SetupRequestDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordRequestDto dto);
    Task<bool> HasUsersAsync();
}
