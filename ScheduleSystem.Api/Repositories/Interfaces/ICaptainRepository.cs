using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface ICaptainRepository
{
    Task<Captain?> GetByDateAndPlanAsync(int ownerId, DateOnly date, int planId);
    Task<Captain> CreateAsync(Captain captain);
    Task DeleteAsync(Captain captain);
    Task<int> CountByEmployeeAndPlanAsync(int ownerId, int employeeId, int planId);
}
