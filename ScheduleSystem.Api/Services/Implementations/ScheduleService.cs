using ScheduleSystem.Api.Models.DTOs.Schedule;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repo;

    public ScheduleService(IScheduleRepository repo) => _repo = repo;

    public async Task<List<ScheduleDto>> GetByDateAsync(int ownerId, DateOnly date) =>
        (await _repo.GetByDateAsync(ownerId, date)).Select(Map).ToList();

    public async Task<List<ScheduleDto>> GetByRangeAsync(int ownerId, DateOnly start, DateOnly end) =>
        (await _repo.GetByRangeAsync(ownerId, start, end)).Select(Map).ToList();

    public async Task<List<ScheduleDto>> GetByEmployeeRangeAsync(int ownerId, int employeeId, DateOnly start, DateOnly end) =>
        (await _repo.GetByEmployeeRangeAsync(ownerId, employeeId, start, end)).Select(Map).ToList();

    public async Task<List<ScheduleDto>> GetByPlanAsync(int ownerId, int planId) =>
        (await _repo.GetByPlanAsync(ownerId, planId)).Select(Map).ToList();

    public async Task<ScheduleDto> CreateAsync(CreateScheduleDto dto, int ownerId, int createdBy)
    {
        if (dto.Type == "work" && await _repo.ExistsWorkScheduleAsync(dto.EmployeeId, dto.Date, dto.PlanId))
            throw new InvalidOperationException("A work schedule already exists for this employee on this date within the same plan.");

        var schedule = await _repo.CreateAsync(new Schedule
        {
            EmployeeId = dto.EmployeeId,
            ShiftId    = dto.ShiftId,
            Date       = dto.Date,
            Type       = dto.Type,
            PlanId     = dto.PlanId,
            CreatedBy  = createdBy,
            OwnerId    = ownerId
        });

        var full = await _repo.GetByDateAsync(ownerId, dto.Date);
        var created = full.FirstOrDefault(s => s.Id == schedule.Id) ?? schedule;
        return Map(created);
    }

    public Task DeleteByRangeAsync(int ownerId, DateOnly start, DateOnly end) =>
        _repo.DeleteByRangeAsync(ownerId, start, end);

    public Task DeleteByPlanAsync(int ownerId, int planId) =>
        _repo.DeleteByPlanAsync(ownerId, planId);

    private static ScheduleDto Map(Schedule s) => new()
    {
        Id           = s.Id,
        EmployeeId   = s.EmployeeId,
        EmployeeName = s.Employee?.Name ?? string.Empty,
        ShiftId      = s.ShiftId,
        ShiftName    = s.Shift?.Name,
        ShiftColor   = s.Shift?.Color,
        Date         = s.Date,
        Type         = s.Type,
        PlanId       = s.PlanId,
        CreatedBy    = s.CreatedBy,
        OwnerId      = s.OwnerId,
        CreatedAt    = s.CreatedAt
    };
}
