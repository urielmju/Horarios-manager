using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllByOwnerAsync(int ownerId);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task<int> CountActiveByOwnerAsync(int ownerId);
}
