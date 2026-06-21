using ScheduleSystem.Api.Models.DTOs.Captain;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class CaptainService : ICaptainService
{
    private readonly ICaptainRepository _repo;

    public CaptainService(ICaptainRepository repo) => _repo = repo;

    public async Task<CaptainDto?> GetByDateAndPlanAsync(int ownerId, DateOnly date, int planId)
    {
        var c = await _repo.GetByDateAndPlanAsync(ownerId, date, planId);
        return c is null ? null : Map(c);
    }

    public async Task<CaptainDto> AssignAsync(AssignCaptainDto dto, int ownerId)
    {
        var existing = await _repo.GetByDateAndPlanAsync(ownerId, dto.Date, dto.PlanId);
        if (existing is not null)
            await _repo.DeleteAsync(existing);

        var captain = await _repo.CreateAsync(new Captain
        {
            EmployeeId = dto.EmployeeId,
            Date       = dto.Date,
            PlanId     = dto.PlanId,
            OwnerId    = ownerId
        });

        return Map(captain);
    }

    public Task<int> GetCountByEmployeeAndPlanAsync(int ownerId, int employeeId, int planId) =>
        _repo.CountByEmployeeAndPlanAsync(ownerId, employeeId, planId);

    private static CaptainDto Map(Captain c) => new()
    {
        Id           = c.Id,
        EmployeeId   = c.EmployeeId,
        EmployeeName = c.Employee?.Name ?? string.Empty,
        Date         = c.Date,
        PlanId       = c.PlanId,
        OwnerId      = c.OwnerId,
        CreatedAt    = c.CreatedAt
    };
}
