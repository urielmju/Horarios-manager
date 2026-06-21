using ScheduleSystem.Api.Models.DTOs.Shift;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IShiftService
{
    Task<List<ShiftDto>> GetAllAsync();
    Task<ShiftDto> GetByIdAsync(int id);
    Task<ShiftDto> CreateAsync(CreateShiftDto dto);
    Task<ShiftDto> UpdateAsync(int id, CreateShiftDto dto);
    Task DeleteAsync(int id);
}
