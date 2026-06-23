using ScheduleSystem.Api.Models.DTOs.Shift;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IShiftService
{
    Task<List<ShiftDto>> GetAllAsync(int ownerId);
    Task<ShiftDto> GetByIdAsync(int id, int ownerId);
    Task<ShiftDto> CreateAsync(CreateShiftDto dto, int ownerId);
    Task<ShiftDto> UpdateAsync(int id, CreateShiftDto dto, int ownerId);
    Task DeleteAsync(int id, int ownerId);
    Task CreateDefaultShiftsAsync(int ownerId);
}
