using ScheduleSystem.Api.Models.DTOs.Department;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repo;

    public DepartmentService(IDepartmentRepository repo) => _repo = repo;

    public async Task<List<DepartmentDto>> GetAllAsync(int ownerId)
    {
        var list = await _repo.GetAllByOwnerAsync(ownerId);
        return list.Select(Map).ToList();
    }

    public async Task<DepartmentDto> GetByIdAsync(int id, int ownerId)
    {
        var d = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Department not found.");
        if (d.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        return Map(d);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, int ownerId)
    {
        var dept = await _repo.CreateAsync(new Department { Name = dto.Name, OwnerId = ownerId });
        return Map(dept);
    }

    public async Task<DepartmentDto> UpdateAsync(int id, CreateDepartmentDto dto, int ownerId)
    {
        var d = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Department not found.");
        if (d.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        d.Name = dto.Name;
        await _repo.UpdateAsync(d);
        return Map(d);
    }

    public async Task DeleteAsync(int id, int ownerId)
    {
        var d = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Department not found.");
        if (d.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        await _repo.DeleteAsync(d);
    }

    private static DepartmentDto Map(Department d) => new()
    {
        Id        = d.Id,
        Name      = d.Name,
        OwnerId   = d.OwnerId,
        CreatedAt = d.CreatedAt
    };
}
