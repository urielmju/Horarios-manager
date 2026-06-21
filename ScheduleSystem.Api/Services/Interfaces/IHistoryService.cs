using ScheduleSystem.Api.Models.DTOs.History;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface IHistoryService
{
    Task<(List<HistoryDto> Items, int Total)> GetPagedAsync(int ownerId, int page, int pageSize);
    Task LogAsync(string action, string details, int ownerId, int? userId = null);
}
