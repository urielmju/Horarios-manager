using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.Department;

public class CreateDepartmentDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
