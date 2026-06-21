using ScheduleSystem.Api.Models.DTOs.Vacation;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class VacationService : IVacationService
{
    private readonly IVacationRepository _repo;

    public VacationService(IVacationRepository repo) => _repo = repo;

    public async Task<List<VacationDto>> GetAllAsync(int ownerId) =>
        (await _repo.GetAllByOwnerAsync(ownerId)).Select(Map).ToList();

    public async Task<List<VacationDto>> GetActiveOnDateAsync(int ownerId, DateOnly date) =>
        (await _repo.GetActiveOnDateAsync(ownerId, date)).Select(Map).ToList();

    public async Task<List<VacationDto>> GetByEmployeeAsync(int ownerId, int employeeId) =>
        (await _repo.GetByEmployeeAsync(ownerId, employeeId)).Select(Map).ToList();

    public async Task<VacationDto> GetByIdAsync(int id, int ownerId)
    {
        var v = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Vacation not found.");
        if (v.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        return Map(v);
    }

    public async Task<VacationDto> CreateAsync(CreateVacationDto dto, int ownerId)
    {
        var v = await _repo.CreateAsync(new Vacation
        {
            EmployeeId = dto.EmployeeId,
            Type       = dto.Type,
            StartDate  = dto.StartDate,
            EndDate    = dto.EndDate,
            Reason     = dto.Reason,
            OwnerId    = ownerId
        });
        return Map(await _repo.GetByIdAsync(v.Id) ?? v);
    }

    public async Task<VacationDto> UpdateAsync(int id, CreateVacationDto dto, int ownerId)
    {
        var v = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Vacation not found.");
        if (v.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        v.EmployeeId = dto.EmployeeId;
        v.Type       = dto.Type;
        v.StartDate  = dto.StartDate;
        v.EndDate    = dto.EndDate;
        v.Reason     = dto.Reason;
        await _repo.UpdateAsync(v);
        return Map(v);
    }

    public async Task DeleteAsync(int id, int ownerId)
    {
        var v = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Vacation not found.");
        if (v.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        await _repo.DeleteAsync(v);
    }

    private static VacationDto Map(Vacation v) => new()
    {
        Id           = v.Id,
        EmployeeId   = v.EmployeeId,
        EmployeeName = v.Employee?.Name ?? string.Empty,
        Type         = v.Type,
        StartDate    = v.StartDate,
        EndDate      = v.EndDate,
        Reason       = v.Reason,
        OwnerId      = v.OwnerId,
        CreatedAt    = v.CreatedAt
    };
}
