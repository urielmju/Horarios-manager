using ScheduleSystem.Api.Models.DTOs.Reports;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IReportService
{
    Task<StatsDto> GetStatsAsync(int ownerId);
}
