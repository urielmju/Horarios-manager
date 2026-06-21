using ScheduleSystem.Api.Models.DTOs.DaysOff;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class DaysOffService : IDaysOffService
{
    private readonly IDaysOffRepository _repo;

    public DaysOffService(IDaysOffRepository repo) => _repo = repo;

    public async Task<List<DaysOffDto>> GetAllAsync(int ownerId) =>
        (await _repo.GetAllByOwnerAsync(ownerId)).Select(Map).ToList();

    public async Task<DaysOffDto> GetByIdAsync(int id, int ownerId)
    {
        var d = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("DaysOff not found.");
        if (d.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        return Map(d);
    }

    public async Task<DaysOffDto> CreateAsync(CreateDaysOffDto dto, int ownerId)
    {
        var entity = new DaysOff
        {
            EmployeeId = dto.EmployeeId,
            Type       = dto.Type,
            OwnerId    = ownerId,
            Entries    = dto.Days.Select(day => new DaysOffEntry { DayOfWeek = day }).ToList()
        };
        var created = await _repo.CreateAsync(entity);
        return Map(await _repo.GetByIdAsync(created.Id) ?? created);
    }

    public async Task<DaysOffDto> UpdateAsync(int id, CreateDaysOffDto dto, int ownerId)
    {
        var d = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("DaysOff not found.");
        if (d.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        d.EmployeeId = dto.EmployeeId;
        d.Type       = dto.Type;
        d.Entries.Clear();
        foreach (var day in dto.Days)
            d.Entries.Add(new DaysOffEntry { DaysOffId = d.Id, DayOfWeek = day });
        await _repo.UpdateAsync(d);
        return Map(d);
    }

    public async Task DeleteAsync(int id, int ownerId)
    {
        var d = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("DaysOff not found.");
        if (d.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        await _repo.DeleteAsync(d);
    }

    private static DaysOffDto Map(DaysOff d) => new()
    {
        Id           = d.Id,
        EmployeeId   = d.EmployeeId,
        EmployeeName = d.Employee?.Name ?? string.Empty,
        Type         = d.Type,
        OwnerId      = d.OwnerId,
        CreatedAt    = d.CreatedAt,
        Days         = d.Entries.Select(e => e.DayOfWeek).ToList()
    };
}
