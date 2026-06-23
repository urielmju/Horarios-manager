using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class ShiftRepository : IShiftRepository
{
    private readonly AppDbContext _db;

    public ShiftRepository(AppDbContext db) => _db = db;

    public Task<List<Shift>> GetAllByOwnerAsync(int ownerId) =>
        _db.Shifts.Where(s => s.OwnerId == ownerId).ToListAsync();

    public Task<Shift?> GetByIdAsync(int id) =>
        _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Shift> CreateAsync(Shift shift)
    {
        _db.Shifts.Add(shift);
        await _db.SaveChangesAsync();
        return shift;
    }

    public async Task CreateManyAsync(IEnumerable<Shift> shifts)
    {
        _db.Shifts.AddRange(shifts);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Shift shift)
    {
        _db.Shifts.Update(shift);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Shift shift)
    {
        _db.Shifts.Remove(shift);
        await _db.SaveChangesAsync();
    }
}
