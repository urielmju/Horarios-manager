namespace ScheduleSystem.Api.Models.DTOs.DaysOff;

public class DaysOffDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<int> Days { get; set; } = new();
}
