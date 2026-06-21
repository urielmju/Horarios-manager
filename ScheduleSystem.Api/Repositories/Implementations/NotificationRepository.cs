using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _db;

    public NotificationRepository(AppDbContext db) => _db = db;

    public Task<List<Notification>> GetAllByOwnerAsync(int ownerId) =>
        _db.Notifications
           .Where(n => n.OwnerId == ownerId)
           .OrderByDescending(n => n.Timestamp)
           .ToListAsync();

    public Task<int> CountUnreadByOwnerAsync(int ownerId) =>
        _db.Notifications.CountAsync(n => n.OwnerId == ownerId && !n.Read);

    public async Task MarkAllReadByOwnerAsync(int ownerId)
    {
        var unread = await _db.Notifications
            .Where(n => n.OwnerId == ownerId && !n.Read)
            .ToListAsync();
        unread.ForEach(n => n.Read = true);
        await _db.SaveChangesAsync();
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
        return notification;
    }

    public async Task DeleteOldestAsync(int ownerId, int keepCount)
    {
        var total = await _db.Notifications.CountAsync(n => n.OwnerId == ownerId);
        if (total <= keepCount) return;

        var toDelete = await _db.Notifications
            .Where(n => n.OwnerId == ownerId)
            .OrderBy(n => n.Timestamp)
            .Take(total - keepCount)
            .ToListAsync();

        _db.Notifications.RemoveRange(toDelete);
        await _db.SaveChangesAsync();
    }
}
