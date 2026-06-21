using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IScheduleRepository
{
    Task<List<Schedule>> GetByDateAsync(int ownerId, DateOnly date);
    Task<List<Schedule>> GetByRangeAsync(int ownerId, DateOnly start, DateOnly end);
    Task<List<Schedule>> GetByEmployeeRangeAsync(int ownerId, int employeeId, DateOnly start, DateOnly end);
    Task<List<Schedule>> GetByPlanAsync(int ownerId, int planId);
    Task<bool> ExistsWorkScheduleAsync(int employeeId, DateOnly date, int planId);
    Task<Schedule> CreateAsync(Schedule schedule);
    Task DeleteByRangeAsync(int ownerId, DateOnly start, DateOnly end);
    Task DeleteByPlanAsync(int ownerId, int planId);
    Task<int> CountScheduledThisWeekAsync(int ownerId, DateOnly weekStart, DateOnly weekEnd);
    Task<int> CountOvertimeAsync(int ownerId, DateOnly weekStart, DateOnly weekEnd);
}
