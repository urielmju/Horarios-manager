using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class VacationRepository : IVacationRepository
{
    private readonly AppDbContext _db;

    public VacationRepository(AppDbContext db) => _db = db;

    public Task<List<Vacation>> GetAllByOwnerAsync(int ownerId) =>
        _db.Vacations.Where(v => v.OwnerId == ownerId).Include(v => v.Employee).ToListAsync();

    public Task<List<Vacation>> GetActiveOnDateAsync(int ownerId, DateOnly date) =>
        _db.Vacations.Where(v => v.OwnerId == ownerId && v.StartDate <= date && v.EndDate >= date)
                     .Include(v => v.Employee).ToListAsync();

    public Task<List<Vacation>> GetByEmployeeAsync(int ownerId, int employeeId) =>
        _db.Vacations.Where(v => v.OwnerId == ownerId && v.EmployeeId == employeeId)
                     .Include(v => v.Employee).ToListAsync();

    public Task<Vacation?> GetByIdAsync(int id) =>
        _db.Vacations.Include(v => v.Employee).FirstOrDefaultAsync(v => v.Id == id);

    public async Task<Vacation> CreateAsync(Vacation vacation)
    {
        _db.Vacations.Add(vacation);
        await _db.SaveChangesAsync();
        return vacation;
    }

    public async Task UpdateAsync(Vacation vacation)
    {
        _db.Vacations.Update(vacation);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Vacation vacation)
    {
        _db.Vacations.Remove(vacation);
        await _db.SaveChangesAsync();
    }

    public Task<int> CountAllAsync() => _db.Vacations.CountAsync();

    public Task<int> CountActiveByOwnerAsync(int ownerId, DateOnly today) =>
        _db.Vacations.CountAsync(v => v.OwnerId == ownerId && v.StartDate <= today && v.EndDate >= today);
}
