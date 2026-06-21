using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IShiftRepository
{
    Task<List<Shift>> GetAllAsync();
    Task<Shift?> GetByIdAsync(int id);
    Task<Shift> CreateAsync(Shift shift);
    Task UpdateAsync(Shift shift);
    Task DeleteAsync(Shift shift);
}
