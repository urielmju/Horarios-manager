using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.DaysOff;

public class CreateDaysOffDto
{
    [Required]
    public int EmployeeId { get; set; }

    [Required, MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    public List<int> Days { get; set; } = new();
}
