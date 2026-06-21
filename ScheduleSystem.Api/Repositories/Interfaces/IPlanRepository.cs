using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IPlanRepository
{
    Task<List<Plan>> GetAllByOwnerAsync(int ownerId);
    Task<Plan?> GetByIdAsync(int id);
    Task<Plan> CreateAsync(Plan plan);
    Task UpdateAsync(Plan plan);
    Task DeleteAsync(Plan plan);
    Task<int> CountByOwnerAsync(int ownerId);
}
