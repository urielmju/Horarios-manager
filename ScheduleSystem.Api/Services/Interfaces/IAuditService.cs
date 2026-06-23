using ScheduleSystem.Api.Models.DTOs.Audit;
using ScheduleSystem.Api.Models.DTOs.Employee;
using ScheduleSystem.Api.Models.DTOs.Plan;
using ScheduleSystem.Api.Models.DTOs.Schedule;
using ScheduleSystem.Api.Models.DTOs.Shift;
using ScheduleSystem.Api.Models.DTOs.Vacation;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IAuditService
{
    Task<List<AuditUserDto>>  GetAllUsersAsync();
    Task<AuditUserDto>        GetUserByIdAsync(int userId);
    Task<List<EmployeeDto>>   GetEmployeesByUserAsync(int userId);
    Task<List<ShiftDto>>      GetShiftsByUserAsync(int userId);
    Task<List<PlanDto>>       GetPlansByUserAsync(int userId);
    Task<List<ScheduleDto>>   GetSchedulesByUserAsync(int userId, DateOnly? start, DateOnly? end);
    Task<List<VacationDto>>   GetVacationsByUserAsync(int userId);
    Task<GlobalStatsDto>      GetGlobalStatsAsync();
}
