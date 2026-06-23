using ScheduleSystem.Api.Models.DTOs.Audit;
using ScheduleSystem.Api.Models.DTOs.Employee;
using ScheduleSystem.Api.Models.DTOs.Plan;
using ScheduleSystem.Api.Models.DTOs.Schedule;
using ScheduleSystem.Api.Models.DTOs.Shift;
using ScheduleSystem.Api.Models.DTOs.Vacation;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class AuditService : IAuditService
{
    private readonly IUserRepository    _userRepo;
    private readonly IEmployeeService   _employees;
    private readonly IShiftService      _shifts;
    private readonly IPlanService       _plans;
    private readonly IScheduleService   _schedules;
    private readonly IVacationService   _vacations;
    private readonly IScheduleRepository _scheduleRepo;
    private readonly IVacationRepository _vacationRepo;

    public AuditService(
        IUserRepository     userRepo,
        IEmployeeService    employees,
        IShiftService       shifts,
        IPlanService        plans,
        IScheduleService    schedules,
        IVacationService    vacations,
        IScheduleRepository scheduleRepo,
        IVacationRepository vacationRepo)
    {
        _userRepo     = userRepo;
        _employees    = employees;
        _shifts       = shifts;
        _plans        = plans;
        _schedules    = schedules;
        _vacations    = vacations;
        _scheduleRepo = scheduleRepo;
        _vacationRepo = vacationRepo;
    }

    public async Task<List<AuditUserDto>> GetAllUsersAsync() =>
        (await _userRepo.GetAllAsync()).Select(MapUser).ToList();

    public async Task<AuditUserDto> GetUserByIdAsync(int userId)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found.");
        return MapUser(user);
    }

    public async Task<List<EmployeeDto>> GetEmployeesByUserAsync(int userId)
    {
        await EnsureUserExistsAsync(userId);
        return await _employees.GetAllAsync(userId);
    }

    public async Task<List<ShiftDto>> GetShiftsByUserAsync(int userId)
    {
        await EnsureUserExistsAsync(userId);
        return await _shifts.GetAllAsync(userId);
    }

    public async Task<List<PlanDto>> GetPlansByUserAsync(int userId)
    {
        await EnsureUserExistsAsync(userId);
        return await _plans.GetAllAsync(userId);
    }

    public async Task<List<ScheduleDto>> GetSchedulesByUserAsync(int userId, DateOnly? start, DateOnly? end)
    {
        await EnsureUserExistsAsync(userId);
        return (start.HasValue && end.HasValue)
            ? await _schedules.GetByRangeAsync(userId, start.Value, end.Value)
            : await _schedules.GetAllAsync(userId);
    }

    public async Task<List<VacationDto>> GetVacationsByUserAsync(int userId)
    {
        await EnsureUserExistsAsync(userId);
        return await _vacations.GetAllAsync(userId);
    }

    public async Task<GlobalStatsDto> GetGlobalStatsAsync()
    {
        var users = await _userRepo.GetAllAsync();
        var breakdown = new List<UserBreakdownDto>();
        int totalEmployees = 0, totalPlans = 0;

        foreach (var user in users)
        {
            var empCount  = (await _employees.GetAllAsync(user.Id)).Count;
            var planCount = (await _plans.GetAllAsync(user.Id)).Count;
            totalEmployees += empCount;
            totalPlans     += planCount;
            breakdown.Add(new UserBreakdownDto
            {
                UserId        = user.Id,
                Username      = user.Username,
                Name          = user.Name,
                EmployeeCount = empCount,
                PlanCount     = planCount
            });
        }

        return new GlobalStatsDto
        {
            TotalUsers     = users.Count,
            TotalEmployees = totalEmployees,
            TotalPlans     = totalPlans,
            TotalSchedules = await _scheduleRepo.CountAllAsync(),
            TotalVacations = await _vacationRepo.CountAllAsync(),
            UserBreakdown  = breakdown
        };
    }

    private async Task EnsureUserExistsAsync(int userId)
    {
        if (await _userRepo.GetByIdAsync(userId) is null)
            throw new KeyNotFoundException("User not found.");
    }

    private static AuditUserDto MapUser(User u) => new()
    {
        Id        = u.Id,
        Username  = u.Username,
        Name      = u.Name,
        Email     = u.Email,
        Role      = u.Role,
        CreatedAt = u.CreatedAt
    };
}
