using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.Plan;

public class CreatePlanDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
