using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) => _db = db;

    public Task<List<Employee>> GetAllByOwnerAsync(int ownerId) =>
        _db.Employees
           .Where(e => e.OwnerId == ownerId)
           .Include(e => e.Department)
           .Include(e => e.AvailableDays)
           .Include(e => e.AvailableShifts)
           .ToListAsync();

    public Task<Employee?> GetByIdAsync(int id) =>
        _db.Employees
           .Include(e => e.Department)
           .Include(e => e.AvailableDays)
           .Include(e => e.AvailableShifts)
           .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Employee> CreateAsync(Employee employee)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteWithRelatedAsync(Employee employee)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var schedules = await _db.Schedules.Where(s => s.EmployeeId == employee.Id).ToListAsync();
            _db.Schedules.RemoveRange(schedules);

            var captains = await _db.Captains.Where(c => c.EmployeeId == employee.Id).ToListAsync();
            _db.Captains.RemoveRange(captains);

            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public Task<int> CountActiveByOwnerAsync(int ownerId) =>
        _db.Employees.CountAsync(e => e.OwnerId == ownerId && e.Active);
}
