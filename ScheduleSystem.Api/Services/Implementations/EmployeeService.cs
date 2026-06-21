using ScheduleSystem.Api.Models.DTOs.Employee;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;

    public EmployeeService(IEmployeeRepository repo) => _repo = repo;

    public async Task<List<EmployeeDto>> GetAllAsync(int ownerId)
    {
        var list = await _repo.GetAllByOwnerAsync(ownerId);
        return list.Select(Map).ToList();
    }

    public async Task<EmployeeDto> GetByIdAsync(int id, int ownerId)
    {
        var e = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Employee not found.");
        if (e.OwnerId != ownerId)
            throw new UnauthorizedAccessException("Access denied.");
        return Map(e);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, int ownerId)
    {
        var employee = new Employee
        {
            Name         = dto.Name,
            Email        = dto.Email,
            Phone        = dto.Phone,
            DepartmentId = dto.DepartmentId,
            Position     = dto.Position,
            MaxHours     = dto.MaxHours,
            OwnerId      = ownerId,
            AvailableDays   = dto.AvailableDays.Select(d => new EmployeeAvailableDay { DayOfWeek = d }).ToList(),
            AvailableShifts = dto.AvailableShifts.Select(s => new EmployeeAvailableShift { ShiftId = s }).ToList()
        };

        var created = await _repo.CreateAsync(employee);
        return Map(await _repo.GetByIdAsync(created.Id) ?? created);
    }

    public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto, int ownerId)
    {
        var e = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Employee not found.");
        if (e.OwnerId != ownerId)
            throw new UnauthorizedAccessException("Access denied.");

        e.Name         = dto.Name;
        e.Email        = dto.Email;
        e.Phone        = dto.Phone;
        e.DepartmentId = dto.DepartmentId;
        e.Position     = dto.Position;
        e.MaxHours     = dto.MaxHours;
        e.AvailableDays.Clear();
        foreach (var d in dto.AvailableDays)
            e.AvailableDays.Add(new EmployeeAvailableDay { EmployeeId = e.Id, DayOfWeek = d });
        e.AvailableShifts.Clear();
        foreach (var s in dto.AvailableShifts)
            e.AvailableShifts.Add(new EmployeeAvailableShift { EmployeeId = e.Id, ShiftId = s });

        await _repo.UpdateAsync(e);
        return Map(await _repo.GetByIdAsync(e.Id) ?? e);
    }

    public async Task ToggleActiveAsync(int id, int ownerId)
    {
        var e = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Employee not found.");
        if (e.OwnerId != ownerId)
            throw new UnauthorizedAccessException("Access denied.");
        e.Active = !e.Active;
        await _repo.UpdateAsync(e);
    }

    public async Task DeleteAsync(int id, int ownerId)
    {
        var e = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Employee not found.");
        if (e.OwnerId != ownerId)
            throw new UnauthorizedAccessException("Access denied.");
        await _repo.DeleteAsync(e);
    }

    private static EmployeeDto Map(Employee e) => new()
    {
        Id             = e.Id,
        Name           = e.Name,
        Email          = e.Email,
        Phone          = e.Phone,
        DepartmentId   = e.DepartmentId,
        DepartmentName = e.Department?.Name,
        Position       = e.Position,
        Active         = e.Active,
        MaxHours       = e.MaxHours,
        OwnerId        = e.OwnerId,
        CreatedAt      = e.CreatedAt,
        AvailableDays   = e.AvailableDays.Select(d => d.DayOfWeek).ToList(),
        AvailableShifts = e.AvailableShifts.Select(s => s.ShiftId).ToList()
    };
}
