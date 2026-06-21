using ScheduleSystem.Api.Models.DTOs.History;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _repo;
    private const int MaxRecords = 500;

    public HistoryService(IHistoryRepository repo) => _repo = repo;

    public async Task<(List<HistoryDto> Items, int Total)> GetPagedAsync(int ownerId, int page, int pageSize)
    {
        var items = await _repo.GetPagedByOwnerAsync(ownerId, page, pageSize);
        var total = await _repo.CountByOwnerAsync(ownerId);
        return (items.Select(Map).ToList(), total);
    }

    public async Task LogAsync(string action, string details, int ownerId, int? userId = null)
    {
        await _repo.CreateAsync(new History
        {
            Action    = action,
            Details   = details,
            UserId    = userId,
            OwnerId   = ownerId,
            Timestamp = DateTime.UtcNow
        });

        await _repo.DeleteOldestAsync(ownerId, MaxRecords);
    }

    private static HistoryDto Map(History h) => new()
    {
        Id        = h.Id,
        Action    = h.Action,
        Details   = h.Details,
        UserId    = h.UserId,
        UserName  = h.User?.Name,
        OwnerId   = h.OwnerId,
        Timestamp = h.Timestamp
    };
}
