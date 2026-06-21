using ScheduleSystem.Api.Models.DTOs.Schedule;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IScheduleService
{
    Task<List<ScheduleDto>> GetByDateAsync(int ownerId, DateOnly date);
    Task<List<ScheduleDto>> GetByRangeAsync(int ownerId, DateOnly start, DateOnly end);
    Task<List<ScheduleDto>> GetByEmployeeRangeAsync(int ownerId, int employeeId, DateOnly start, DateOnly end);
    Task<List<ScheduleDto>> GetByPlanAsync(int ownerId, int planId);
    Task<ScheduleDto> CreateAsync(CreateScheduleDto dto, int ownerId, int createdBy);
    Task DeleteByRangeAsync(int ownerId, DateOnly start, DateOnly end);
    Task DeleteByPlanAsync(int ownerId, int planId);
}
