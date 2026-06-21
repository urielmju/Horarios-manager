using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleSystem.Api.Models.Entities;

public class History
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    [Required, MaxLength(300)]
    public string Details { get; set; } = string.Empty;

    public int? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public User Owner { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
