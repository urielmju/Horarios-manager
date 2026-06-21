using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleSystem.Api.Models.Entities;

public class Schedule
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;

    public int? ShiftId { get; set; }

    [ForeignKey(nameof(ShiftId))]
    public Shift? Shift { get; set; }

    public DateOnly Date { get; set; }

    [Required, MaxLength(10)]
    public string Type { get; set; } = string.Empty;

    public int PlanId { get; set; }

    [ForeignKey(nameof(PlanId))]
    public Plan Plan { get; set; } = null!;

    public int CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public User Creator { get; set; } = null!;

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public User Owner { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
