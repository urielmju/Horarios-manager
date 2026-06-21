using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetAllByOwnerAsync(int ownerId);
    Task<int> CountUnreadByOwnerAsync(int ownerId);
    Task MarkAllReadByOwnerAsync(int ownerId);
    Task<Notification> CreateAsync(Notification notification);
    Task DeleteOldestAsync(int ownerId, int keepCount);
}
