namespace ScheduleSystem.Api.Models.DTOs.Audit;

public class GlobalStatsDto
{
    public int                    TotalUsers     { get; set; }
    public int                    TotalEmployees { get; set; }
    public int                    TotalPlans     { get; set; }
    public int                    TotalSchedules { get; set; }
    public int                    TotalVacations { get; set; }
    public List<UserBreakdownDto> UserBreakdown  { get; set; } = [];
}

public class UserBreakdownDto
{
    public int    UserId        { get; set; }
    public string Username      { get; set; } = string.Empty;
    public string Name          { get; set; } = string.Empty;
    public int    EmployeeCount { get; set; }
    public int    PlanCount     { get; set; }
}
