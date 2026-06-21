using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.Entities;

public class Shift
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    [Required, MaxLength(10)]
    public string Color { get; set; } = string.Empty;

    public int Hours { get; set; }
}
