using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class ScheduleRepository : IScheduleRepository
{
    private readonly AppDbContext _db;

    public ScheduleRepository(AppDbContext db) => _db = db;

    private IQueryable<Schedule> OwnerQuery(int ownerId) =>
        _db.Schedules
           .Where(s => s.OwnerId == ownerId)
           .Include(s => s.Employee)
           .Include(s => s.Shift);

    public Task<List<Schedule>> GetByDateAsync(int ownerId, DateOnly date) =>
        OwnerQuery(ownerId).Where(s => s.Date == date).ToListAsync();

    public Task<List<Schedule>> GetByRangeAsync(int ownerId, DateOnly start, DateOnly end) =>
        OwnerQuery(ownerId).Where(s => s.Date >= start && s.Date <= end).ToListAsync();

    public Task<List<Schedule>> GetByEmployeeRangeAsync(int ownerId, int employeeId, DateOnly start, DateOnly end) =>
        OwnerQuery(ownerId).Where(s => s.EmployeeId == employeeId && s.Date >= start && s.Date <= end).ToListAsync();

    public Task<List<Schedule>> GetByPlanAsync(int ownerId, int planId) =>
        OwnerQuery(ownerId).Where(s => s.PlanId == planId).ToListAsync();

    public Task<bool> ExistsWorkScheduleAsync(int employeeId, DateOnly date, int planId) =>
        _db.Schedules.AnyAsync(s => s.EmployeeId == employeeId && s.Date == date && s.PlanId == planId && s.Type == "work");

    public async Task<Schedule> CreateAsync(Schedule schedule)
    {
        _db.Schedules.Add(schedule);
        await _db.SaveChangesAsync();
        return schedule;
    }

    public async Task DeleteByRangeAsync(int ownerId, DateOnly start, DateOnly end)
    {
        var items = await _db.Schedules
            .Where(s => s.OwnerId == ownerId && s.Date >= start && s.Date <= end)
            .ToListAsync();
        _db.Schedules.RemoveRange(items);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteByPlanAsync(int ownerId, int planId)
    {
        var items = await _db.Schedules
            .Where(s => s.OwnerId == ownerId && s.PlanId == planId)
            .ToListAsync();
        _db.Schedules.RemoveRange(items);
        await _db.SaveChangesAsync();
    }

    public Task<int> CountScheduledThisWeekAsync(int ownerId, DateOnly weekStart, DateOnly weekEnd) =>
        _db.Schedules.CountAsync(s => s.OwnerId == ownerId && s.Date >= weekStart && s.Date <= weekEnd && s.Type == "work");

    public async Task<int> CountOvertimeAsync(int ownerId, DateOnly weekStart, DateOnly weekEnd)
    {
        var employeeHours = await _db.Schedules
            .Where(s => s.OwnerId == ownerId && s.Date >= weekStart && s.Date <= weekEnd && s.Type == "work")
            .Include(s => s.Shift)
            .GroupBy(s => s.EmployeeId)
            .Select(g => new { EmployeeId = g.Key, TotalHours = g.Sum(s => s.Shift != null ? s.Shift.Hours : 0) })
            .ToListAsync();

        return employeeHours.Count(e => e.TotalHours > 48);
    }
}
