using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.Schedule;

public class CreateScheduleDto
{
    [Required]
    public int EmployeeId { get; set; }

    public int? ShiftId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required, MaxLength(10)]
    public string Type { get; set; } = string.Empty;

    [Required]
    public int PlanId { get; set; }
}
