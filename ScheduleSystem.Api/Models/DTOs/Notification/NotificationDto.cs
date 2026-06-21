namespace ScheduleSystem.Api.Models.DTOs.Notification;

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Read { get; set; }
    public int OwnerId { get; set; }
    public DateTime Timestamp { get; set; }
}
