using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.Vacation;

public class CreateVacationDto
{
    [Required]
    public int EmployeeId { get; set; }

    [Required, MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [MaxLength(300)]
    public string? Reason { get; set; }
}
