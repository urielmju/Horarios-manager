namespace ScheduleSystem.Api.Models.DTOs.Captain;

public class CaptainDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int PlanId { get; set; }
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}
