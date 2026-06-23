using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Models.Entities;

namespace ScheduleSystem.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeeAvailableDay> EmployeeAvailableDays => Set<EmployeeAvailableDay>();
    public DbSet<EmployeeAvailableShift> EmployeeAvailableShifts => Set<EmployeeAvailableShift>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Vacation> Vacations => Set<Vacation>();
    public DbSet<DaysOff> DaysOff => Set<DaysOff>();
    public DbSet<DaysOffEntry> DaysOffEntries => Set<DaysOffEntry>();
    public DbSet<Captain> Captains => Set<Captain>();
    public DbSet<History> History => Set<History>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<EmployeeAvailableDay>(e =>
        {
            e.HasKey(x => new { x.EmployeeId, x.DayOfWeek });
            e.HasOne(x => x.Employee)
             .WithMany(emp => emp.AvailableDays)
             .HasForeignKey(x => x.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EmployeeAvailableShift>(e =>
        {
            e.HasKey(x => new { x.EmployeeId, x.ShiftId });
            e.HasOne(x => x.Employee)
             .WithMany(emp => emp.AvailableShifts)
             .HasForeignKey(x => x.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Shift)
             .WithMany()
             .HasForeignKey(x => x.ShiftId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Shift>(e =>
        {
            e.HasOne(s => s.Owner)
             .WithMany()
             .HasForeignKey(s => s.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasOne(emp => emp.Owner)
             .WithMany()
             .HasForeignKey(emp => emp.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(emp => emp.Department)
             .WithMany(d => d.Employees)
             .HasForeignKey(emp => emp.DepartmentId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Department>(e =>
        {
            e.HasOne(d => d.Owner)
             .WithMany()
             .HasForeignKey(d => d.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Plan>(e =>
        {
            e.HasOne(p => p.Owner)
             .WithMany()
             .HasForeignKey(p => p.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Schedule>(e =>
        {
            e.HasOne(s => s.Employee)
             .WithMany()
             .HasForeignKey(s => s.EmployeeId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Shift)
             .WithMany()
             .HasForeignKey(s => s.ShiftId)
             .OnDelete(DeleteBehavior.SetNull);
            e.HasOne(s => s.Plan)
             .WithMany(p => p.Schedules)
             .HasForeignKey(s => s.PlanId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(s => s.Creator)
             .WithMany()
             .HasForeignKey(s => s.CreatedBy)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Owner)
             .WithMany()
             .HasForeignKey(s => s.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Vacation>(e =>
        {
            e.HasOne(v => v.Employee)
             .WithMany()
             .HasForeignKey(v => v.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(v => v.Owner)
             .WithMany()
             .HasForeignKey(v => v.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DaysOff>(e =>
        {
            e.HasOne(d => d.Employee)
             .WithMany()
             .HasForeignKey(d => d.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(d => d.Owner)
             .WithMany()
             .HasForeignKey(d => d.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DaysOffEntry>(e =>
        {
            e.HasKey(x => new { x.DaysOffId, x.DayOfWeek });
            e.HasOne(x => x.DaysOff)
             .WithMany(d => d.Entries)
             .HasForeignKey(x => x.DaysOffId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Captain>(e =>
        {
            e.HasOne(c => c.Employee)
             .WithMany()
             .HasForeignKey(c => c.EmployeeId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Plan)
             .WithMany(p => p.Captains)
             .HasForeignKey(c => c.PlanId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(c => c.Owner)
             .WithMany()
             .HasForeignKey(c => c.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<History>(e =>
        {
            e.HasOne(h => h.User)
             .WithMany()
             .HasForeignKey(h => h.UserId)
             .OnDelete(DeleteBehavior.SetNull);
            e.HasOne(h => h.Owner)
             .WithMany()
             .HasForeignKey(h => h.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.HasOne(n => n.Owner)
             .WithMany()
             .HasForeignKey(n => n.OwnerId)
             .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
