using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface IHistoryRepository
{
    Task<List<History>> GetPagedByOwnerAsync(int ownerId, int page, int pageSize);
    Task<int> CountByOwnerAsync(int ownerId);
    Task<History> CreateAsync(History history);
    Task DeleteOldestAsync(int ownerId, int keepCount);
}
