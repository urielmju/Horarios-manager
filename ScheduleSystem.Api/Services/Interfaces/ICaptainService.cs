using ScheduleSystem.Api.Models.DTOs.Captain;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface ICaptainService
{
    Task<List<CaptainDto>> GetAllAsync(int ownerId);
    Task<CaptainDto?> GetByDateAndPlanAsync(int ownerId, DateOnly date, int planId);
    Task<CaptainDto> AssignAsync(AssignCaptainDto dto, int ownerId);
    Task<int> GetCountByEmployeeAndPlanAsync(int ownerId, int employeeId, int planId);
}
