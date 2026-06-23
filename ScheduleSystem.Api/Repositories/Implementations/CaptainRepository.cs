using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class CaptainRepository : ICaptainRepository
{
    private readonly AppDbContext _db;

    public CaptainRepository(AppDbContext db) => _db = db;

    public Task<List<Captain>> GetAllByOwnerAsync(int ownerId) =>
        _db.Captains
           .Where(c => c.OwnerId == ownerId)
           .Include(c => c.Employee)
           .ToListAsync();

    public Task<Captain?> GetByDateAndPlanAsync(int ownerId, DateOnly date, int planId) =>
        _db.Captains
           .Include(c => c.Employee)
           .FirstOrDefaultAsync(c => c.OwnerId == ownerId && c.Date == date && c.PlanId == planId);

    public async Task<Captain> CreateAsync(Captain captain)
    {
        _db.Captains.Add(captain);
        await _db.SaveChangesAsync();
        return captain;
    }

    public async Task DeleteAsync(Captain captain)
    {
        _db.Captains.Remove(captain);
        await _db.SaveChangesAsync();
    }

    public Task<int> CountByEmployeeAndPlanAsync(int ownerId, int employeeId, int planId) =>
        _db.Captains.CountAsync(c => c.OwnerId == ownerId && c.EmployeeId == employeeId && c.PlanId == planId);
}
