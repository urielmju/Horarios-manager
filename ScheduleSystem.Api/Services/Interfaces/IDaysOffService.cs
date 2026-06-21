using ScheduleSystem.Api.Models.DTOs.DaysOff;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IDaysOffService
{
    Task<List<DaysOffDto>> GetAllAsync(int ownerId);
    Task<DaysOffDto> GetByIdAsync(int id, int ownerId);
    Task<DaysOffDto> CreateAsync(CreateDaysOffDto dto, int ownerId);
    Task<DaysOffDto> UpdateAsync(int id, CreateDaysOffDto dto, int ownerId);
    Task DeleteAsync(int id, int ownerId);
}
