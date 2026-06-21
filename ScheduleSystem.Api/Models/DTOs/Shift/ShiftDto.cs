namespace ScheduleSystem.Api.Models.DTOs.Shift;

public class ShiftDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Hours { get; set; }
}
