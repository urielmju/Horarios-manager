using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleSystem.Api.Models.Entities;

public class Plan
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public User Owner { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<Captain> Captains { get; set; } = new List<Captain>();
}
