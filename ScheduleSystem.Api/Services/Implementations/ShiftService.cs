using ScheduleSystem.Api.Models.DTOs.Shift;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class ShiftService : IShiftService
{
    private readonly IShiftRepository _repo;

    public ShiftService(IShiftRepository repo) => _repo = repo;

    public async Task<List<ShiftDto>> GetAllAsync()
    {
        var list = await _repo.GetAllAsync();
        return list.Select(Map).ToList();
    }

    public async Task<ShiftDto> GetByIdAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Shift not found.");
        return Map(s);
    }

    public async Task<ShiftDto> CreateAsync(CreateShiftDto dto)
    {
        var shift = await _repo.CreateAsync(new Shift
        {
            Name      = dto.Name,
            StartTime = dto.StartTime,
            EndTime   = dto.EndTime,
            Color     = dto.Color,
            Hours     = dto.Hours
        });
        return Map(shift);
    }

    public async Task<ShiftDto> UpdateAsync(int id, CreateShiftDto dto)
    {
        var s = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Shift not found.");
        s.Name      = dto.Name;
        s.StartTime = dto.StartTime;
        s.EndTime   = dto.EndTime;
        s.Color     = dto.Color;
        s.Hours     = dto.Hours;
        await _repo.UpdateAsync(s);
        return Map(s);
    }

    public async Task DeleteAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Shift not found.");
        await _repo.DeleteAsync(s);
    }

    private static ShiftDto Map(Shift s) => new()
    {
        Id        = s.Id,
        Name      = s.Name,
        StartTime = s.StartTime,
        EndTime   = s.EndTime,
        Color     = s.Color,
        Hours     = s.Hours
    };
}
