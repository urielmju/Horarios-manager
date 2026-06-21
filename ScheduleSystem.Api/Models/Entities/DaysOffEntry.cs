using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleSystem.Api.Models.Entities;

public class DaysOffEntry
{
    public int DaysOffId { get; set; }

    [ForeignKey(nameof(DaysOffId))]
    public DaysOff DaysOff { get; set; } = null!;

    public int DayOfWeek { get; set; }
}
