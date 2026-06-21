using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IDaysOffRepository
{
    Task<List<DaysOff>> GetAllByOwnerAsync(int ownerId);
    Task<DaysOff?> GetByIdAsync(int id);
    Task<DaysOff> CreateAsync(DaysOff daysOff);
    Task UpdateAsync(DaysOff daysOff);
    Task DeleteAsync(DaysOff daysOff);
}
