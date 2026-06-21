using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.Employee;

public class UpdateEmployeeDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    public int? DepartmentId { get; set; }

    [MaxLength(100)]
    public string? Position { get; set; }

    public int MaxHours { get; set; } = 48;

    public List<int> AvailableDays { get; set; } = new();

    public List<int> AvailableShifts { get; set; } = new();
}
