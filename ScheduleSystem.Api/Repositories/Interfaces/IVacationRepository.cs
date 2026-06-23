using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IVacationRepository
{
    Task<List<Vacation>> GetAllByOwnerAsync(int ownerId);
    Task<List<Vacation>> GetActiveOnDateAsync(int ownerId, DateOnly date);
    Task<List<Vacation>> GetByEmployeeAsync(int ownerId, int employeeId);
    Task<Vacation?> GetByIdAsync(int id);
    Task<Vacation> CreateAsync(Vacation vacation);
    Task UpdateAsync(Vacation vacation);
    Task DeleteAsync(Vacation vacation);
    Task<int> CountAllAsync();
    Task<int> CountActiveByOwnerAsync(int ownerId, DateOnly today);
}
