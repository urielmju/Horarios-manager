using ScheduleSystem.Api.Models.DTOs.Plan;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IPlanService
{
    Task<List<PlanDto>> GetAllAsync(int ownerId);
    Task<PlanDto> GetByIdAsync(int id, int ownerId);
    Task<PlanDto> CreateAsync(CreatePlanDto dto, int ownerId);
    Task<PlanDto> UpdateAsync(int id, CreatePlanDto dto, int ownerId);
    Task DeleteAsync(int id, int ownerId);
}
