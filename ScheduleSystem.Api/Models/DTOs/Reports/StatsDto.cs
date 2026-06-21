namespace ScheduleSystem.Api.Models.DTOs.Reports;

public class StatsDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int OnLeave { get; set; }
    public int ScheduledThisWeek { get; set; }
    public int Overtime { get; set; }
    public int TotalPlans { get; set; }
    public int Departments { get; set; }
}
