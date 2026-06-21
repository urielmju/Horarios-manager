namespace ScheduleSystem.Api.Models.DTOs.History;

public class HistoryDto
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public int OwnerId { get; set; }
    public DateTime Timestamp { get; set; }
}
