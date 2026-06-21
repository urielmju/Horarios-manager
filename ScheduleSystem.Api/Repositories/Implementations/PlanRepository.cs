using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class PlanRepository : IPlanRepository
{
    private readonly AppDbContext _db;

    public PlanRepository(AppDbContext db) => _db = db;

    public Task<List<Plan>> GetAllByOwnerAsync(int ownerId) =>
        _db.Plans.Where(p => p.OwnerId == ownerId).ToListAsync();

    public Task<Plan?> GetByIdAsync(int id) =>
        _db.Plans.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Plan> CreateAsync(Plan plan)
    {
        _db.Plans.Add(plan);
        await _db.SaveChangesAsync();
        return plan;
    }

    public async Task UpdateAsync(Plan plan)
    {
        _db.Plans.Update(plan);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Plan plan)
    {
        _db.Plans.Remove(plan);
        await _db.SaveChangesAsync();
    }

    public Task<int> CountByOwnerAsync(int ownerId) =>
        _db.Plans.CountAsync(p => p.OwnerId == ownerId);
}
