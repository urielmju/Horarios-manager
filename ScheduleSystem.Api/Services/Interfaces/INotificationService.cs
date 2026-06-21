using ScheduleSystem.Api.Models.DTOs.Notification;

namespace ScheduleSystem.Api.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetAllAsync(int ownerId);
    Task<int> GetUnreadCountAsync(int ownerId);
    Task MarkAllReadAsync(int ownerId);
    Task CreateAsync(string title, string body, string type, int ownerId);
}
