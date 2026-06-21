using ScheduleSystem.Api.Models.DTOs.Department;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync(int ownerId);
    Task<DepartmentDto> GetByIdAsync(int id, int ownerId);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, int ownerId);
    Task<DepartmentDto> UpdateAsync(int id, CreateDepartmentDto dto, int ownerId);
    Task DeleteAsync(int id, int ownerId);
}
