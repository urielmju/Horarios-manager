using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllByOwnerAsync(int ownerId);
    Task<Department?> GetByIdAsync(int id);
    Task<Department> CreateAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(Department department);
    Task CreateManyAsync(IEnumerable<Department> departments);
    Task<int> CountByOwnerAsync(int ownerId);
}
