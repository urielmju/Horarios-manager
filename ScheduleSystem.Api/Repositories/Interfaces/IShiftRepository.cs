using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IShiftRepository
{
    Task<List<Shift>> GetAllByOwnerAsync(int ownerId);
    Task<Shift?> GetByIdAsync(int id);
    Task<Shift> CreateAsync(Shift shift);
    Task CreateManyAsync(IEnumerable<Shift> shifts);
    Task UpdateAsync(Shift shift);
    Task DeleteAsync(Shift shift);
}
