namespace ScheduleSystem.Api.Models.DTOs.Employee;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string? Position { get; set; }
    public bool Active { get; set; }
    public int MaxHours { get; set; }
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<int> AvailableDays { get; set; } = new();
    public List<int> AvailableShifts { get; set; } = new();
}
