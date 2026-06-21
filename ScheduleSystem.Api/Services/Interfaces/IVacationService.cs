using ScheduleSystem.Api.Models.DTOs.Vacation;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IVacationService
{
    Task<List<VacationDto>> GetAllAsync(int ownerId);
    Task<List<VacationDto>> GetActiveOnDateAsync(int ownerId, DateOnly date);
    Task<List<VacationDto>> GetByEmployeeAsync(int ownerId, int employeeId);
    Task<VacationDto> GetByIdAsync(int id, int ownerId);
    Task<VacationDto> CreateAsync(CreateVacationDto dto, int ownerId);
    Task<VacationDto> UpdateAsync(int id, CreateVacationDto dto, int ownerId);
    Task DeleteAsync(int id, int ownerId);
}
