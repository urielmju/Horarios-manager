using System.ComponentModel.DataAnnotations;

namespace ScheduleSystem.Api.Models.DTOs.Captain;

public class AssignCaptainDto
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public int PlanId { get; set; }
}
