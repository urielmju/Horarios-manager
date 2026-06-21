using ScheduleSystem.Api.Models.DTOs.Plan;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _repo;

    public PlanService(IPlanRepository repo) => _repo = repo;

    public async Task<List<PlanDto>> GetAllAsync(int ownerId)
    {
        var list = await _repo.GetAllByOwnerAsync(ownerId);
        return list.Select(Map).ToList();
    }

    public async Task<PlanDto> GetByIdAsync(int id, int ownerId)
    {
        var p = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Plan not found.");
        if (p.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        return Map(p);
    }

    public async Task<PlanDto> CreateAsync(CreatePlanDto dto, int ownerId)
    {
        var plan = await _repo.CreateAsync(new Plan
        {
            Name        = dto.Name,
            StartDate   = dto.StartDate,
            EndDate     = dto.EndDate,
            Description = dto.Description,
            OwnerId     = ownerId
        });
        return Map(plan);
    }

    public async Task<PlanDto> UpdateAsync(int id, CreatePlanDto dto, int ownerId)
    {
        var p = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Plan not found.");
        if (p.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        p.Name        = dto.Name;
        p.StartDate   = dto.StartDate;
        p.EndDate     = dto.EndDate;
        p.Description = dto.Description;
        await _repo.UpdateAsync(p);
        return Map(p);
    }

    public async Task DeleteAsync(int id, int ownerId)
    {
        var p = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Plan not found.");
        if (p.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        await _repo.DeleteAsync(p);
    }

    private static PlanDto Map(Plan p) => new()
    {
        Id          = p.Id,
        Name        = p.Name,
        StartDate   = p.StartDate,
        EndDate     = p.EndDate,
        Description = p.Description,
        OwnerId     = p.OwnerId,
        CreatedAt   = p.CreatedAt
    };
}
