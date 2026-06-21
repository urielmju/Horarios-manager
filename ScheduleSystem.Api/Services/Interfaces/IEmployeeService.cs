using ScheduleSystem.Api.Models.DTOs.Employee;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllAsync(int ownerId);
    Task<EmployeeDto> GetByIdAsync(int id, int ownerId);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, int ownerId);
    Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto, int ownerId);
    Task ToggleActiveAsync(int id, int ownerId);
    Task DeleteAsync(int id, int ownerId);
}
