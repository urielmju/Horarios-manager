using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleSystem.Api.Models.Entities;

public class EmployeeAvailableShift
{
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;

    public int ShiftId { get; set; }

    [ForeignKey(nameof(ShiftId))]
    public Shift Shift { get; set; } = null!;
}
