using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleSystem.Api.Models.Entities;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(300)]
    public string Body { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public bool Read { get; set; } = false;

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public User Owner { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
