using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.Shift;

public class CreateShiftDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    [Required, MaxLength(10)]
    public string Color { get; set; } = string.Empty;

    [Required]
    public int Hours { get; set; }
}
