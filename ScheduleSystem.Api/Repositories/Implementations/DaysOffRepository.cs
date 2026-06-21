using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class DaysOffRepository : IDaysOffRepository
{
    private readonly AppDbContext _db;

    public DaysOffRepository(AppDbContext db) => _db = db;

    public Task<List<DaysOff>> GetAllByOwnerAsync(int ownerId) =>
        _db.DaysOff.Where(d => d.OwnerId == ownerId)
                   .Include(d => d.Employee)
                   .Include(d => d.Entries)
                   .ToListAsync();

    public Task<DaysOff?> GetByIdAsync(int id) =>
        _db.DaysOff.Include(d => d.Employee).Include(d => d.Entries).FirstOrDefaultAsync(d => d.Id == id);

    public async Task<DaysOff> CreateAsync(DaysOff daysOff)
    {
        _db.DaysOff.Add(daysOff);
        await _db.SaveChangesAsync();
        return daysOff;
    }

    public async Task UpdateAsync(DaysOff daysOff)
    {
        _db.DaysOff.Update(daysOff);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(DaysOff daysOff)
    {
        _db.DaysOff.Remove(daysOff);
        await _db.SaveChangesAsync();
    }
}
