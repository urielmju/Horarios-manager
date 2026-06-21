using ScheduleSystem.Api.Models.DTOs.Reports;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IEmployeeRepository  _employees;
    private readonly IVacationRepository  _vacations;
    private readonly IScheduleRepository  _schedules;
    private readonly IPlanRepository      _plans;
    private readonly IDepartmentRepository _departments;

    public ReportService(
        IEmployeeRepository employees,
        IVacationRepository vacations,
        IScheduleRepository schedules,
        IPlanRepository plans,
        IDepartmentRepository departments)
    {
        _employees   = employees;
        _vacations   = vacations;
        _schedules   = schedules;
        _plans       = plans;
        _departments = departments;
    }

    public async Task<StatsDto> GetStatsAsync(int ownerId)
    {
        var today     = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var weekEnd   = weekStart.AddDays(6);

        var allEmployees = await _employees.GetAllByOwnerAsync(ownerId);

        return new StatsDto
        {
            TotalEmployees    = allEmployees.Count,
            ActiveEmployees   = allEmployees.Count(e => e.Active),
            OnLeave           = await _vacations.CountActiveByOwnerAsync(ownerId, today),
            ScheduledThisWeek = await _schedules.CountScheduledThisWeekAsync(ownerId, weekStart, weekEnd),
            Overtime          = await _schedules.CountOvertimeAsync(ownerId, weekStart, weekEnd),
            TotalPlans        = await _plans.CountByOwnerAsync(ownerId),
            Departments       = await _departments.CountByOwnerAsync(ownerId)
        };
    }
}
