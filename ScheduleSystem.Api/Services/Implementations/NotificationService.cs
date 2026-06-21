using ScheduleSystem.Api.Models.DTOs.Notification;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private const int MaxRecords = 100;

    public NotificationService(INotificationRepository repo) => _repo = repo;

    public async Task<List<NotificationDto>> GetAllAsync(int ownerId) =>
        (await _repo.GetAllByOwnerAsync(ownerId)).Select(Map).ToList();

    public Task<int> GetUnreadCountAsync(int ownerId) =>
        _repo.CountUnreadByOwnerAsync(ownerId);

    public Task MarkAllReadAsync(int ownerId) =>
        _repo.MarkAllReadByOwnerAsync(ownerId);

    public async Task CreateAsync(string title, string body, string type, int ownerId)
    {
        await _repo.CreateAsync(new Notification
        {
            Title     = title,
            Body      = body,
            Type      = type,
            OwnerId   = ownerId,
            Timestamp = DateTime.UtcNow
        });
        await _repo.DeleteOldestAsync(ownerId, MaxRecords);
    }

    private static NotificationDto Map(Notification n) => new()
    {
        Id        = n.Id,
        Title     = n.Title,
        Body      = n.Body,
        Type      = n.Type,
        Read      = n.Read,
        OwnerId   = n.OwnerId,
        Timestamp = n.Timestamp
    };
}
