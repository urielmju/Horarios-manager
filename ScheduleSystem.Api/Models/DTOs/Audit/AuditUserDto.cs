namespace ScheduleSystem.Api.Models.DTOs.Audit;

public class AuditUserDto
{
    public int      Id        { get; set; }
    public string   Username  { get; set; } = string.Empty;
    public string   Name      { get; set; } = string.Empty;
    public string   Email     { get; set; } = string.Empty;
    public string   Role      { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
