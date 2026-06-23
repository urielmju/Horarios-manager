using ScheduleSystem.Api.Models.DTOs.Shift;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class ShiftService : IShiftService
{
    private readonly IShiftRepository _repo;

    public ShiftService(IShiftRepository repo) => _repo = repo;

    public async Task<List<ShiftDto>> GetAllAsync(int ownerId) =>
        (await _repo.GetAllByOwnerAsync(ownerId)).Select(Map).ToList();

    public async Task<ShiftDto> GetByIdAsync(int id, int ownerId)
    {
        var s = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Shift not found.");
        if (s.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        return Map(s);
    }

    public async Task<ShiftDto> CreateAsync(CreateShiftDto dto, int ownerId)
    {
        var shift = await _repo.CreateAsync(new Shift
        {
            Name      = dto.Name,
            StartTime = dto.StartTime,
            EndTime   = dto.EndTime,
            Color     = dto.Color,
            Hours     = dto.Hours,
            OwnerId   = ownerId
        });
        return Map(shift);
    }

    public async Task<ShiftDto> UpdateAsync(int id, CreateShiftDto dto, int ownerId)
    {
        var s = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Shift not found.");
        if (s.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        s.Name      = dto.Name;
        s.StartTime = dto.StartTime;
        s.EndTime   = dto.EndTime;
        s.Color     = dto.Color;
        s.Hours     = dto.Hours;
        await _repo.UpdateAsync(s);
        return Map(s);
    }

    public async Task DeleteAsync(int id, int ownerId)
    {
        var s = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Shift not found.");
        if (s.OwnerId != ownerId) throw new UnauthorizedAccessException("Access denied.");
        await _repo.DeleteAsync(s);
    }

    public Task CreateDefaultShiftsAsync(int ownerId)
    {
        var defaults = new[]
        {
            new Shift { Name = "Morning",   StartTime = new TimeSpan(8,  0, 0), EndTime = new TimeSpan(16, 0, 0), Color = "#F6AD55", Hours = 8, OwnerId = ownerId },
            new Shift { Name = "Afternoon", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(20, 0, 0), Color = "#68D391", Hours = 8, OwnerId = ownerId },
            new Shift { Name = "Night",     StartTime = new TimeSpan(20, 0, 0), EndTime = new TimeSpan(4,  0, 0), Color = "#76E4F7", Hours = 8, OwnerId = ownerId },
        };
        return _repo.CreateManyAsync(defaults);
    }

    private static ShiftDto Map(Shift s) => new()
    {
        Id        = s.Id,
        Name      = s.Name,
        StartTime = s.StartTime,
        EndTime   = s.EndTime,
        Color     = s.Color,
        Hours     = s.Hours,
        OwnerId   = s.OwnerId
    };
}
