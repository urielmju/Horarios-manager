using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class HistoryRepository : IHistoryRepository
{
    private readonly AppDbContext _db;

    public HistoryRepository(AppDbContext db) => _db = db;

    public Task<List<History>> GetPagedByOwnerAsync(int ownerId, int page, int pageSize) =>
        _db.History
           .Where(h => h.OwnerId == ownerId)
           .Include(h => h.User)
           .OrderByDescending(h => h.Timestamp)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

    public Task<int> CountByOwnerAsync(int ownerId) =>
        _db.History.CountAsync(h => h.OwnerId == ownerId);

    public async Task<History> CreateAsync(History history)
    {
        _db.History.Add(history);
        await _db.SaveChangesAsync();
        return history;
    }

    public async Task DeleteOldestAsync(int ownerId, int keepCount)
    {
        var total = await _db.History.CountAsync(h => h.OwnerId == ownerId);
        if (total <= keepCount) return;

        var toDelete = await _db.History
            .Where(h => h.OwnerId == ownerId)
            .OrderBy(h => h.Timestamp)
            .Take(total - keepCount)
            .ToListAsync();

        _db.History.RemoveRange(toDelete);
        await _db.SaveChangesAsync();
    }
}
