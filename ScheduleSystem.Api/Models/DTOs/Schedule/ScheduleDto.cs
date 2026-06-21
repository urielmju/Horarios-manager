namespace ScheduleSystem.Api.Models.DTOs.Schedule;

public class ScheduleDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int? ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public string? ShiftColor { get; set; }
    public DateOnly Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public int PlanId { get; set; }
    public int CreatedBy { get; set; }
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}
