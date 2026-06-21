using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleSystem.Api.Models.Entities;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    public int? DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public Department? Department { get; set; }

    [MaxLength(100)]
    public string? Position { get; set; }

    public bool Active { get; set; } = true;

    public int MaxHours { get; set; } = 48;

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public User Owner { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<EmployeeAvailableDay> AvailableDays { get; set; } = new List<EmployeeAvailableDay>();
    public ICollection<EmployeeAvailableShift> AvailableShifts { get; set; } = new List<EmployeeAvailableShift>();
}
