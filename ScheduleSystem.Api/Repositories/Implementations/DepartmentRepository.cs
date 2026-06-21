using Microsoft.EntityFrameworkCore;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;

namespace ScheduleSystem.Api.Repositories.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _db;

    public DepartmentRepository(AppDbContext db) => _db = db;

    public Task<List<Department>> GetAllByOwnerAsync(int ownerId) =>
        _db.Departments.Where(d => d.OwnerId == ownerId).ToListAsync();

    public Task<Department?> GetByIdAsync(int id) =>
        _db.Departments.FirstOrDefaultAsync(d => d.Id == id);

    public async Task<Department> CreateAsync(Department department)
    {
        _db.Departments.Add(department);
        await _db.SaveChangesAsync();
        return department;
    }

    public async Task UpdateAsync(Department department)
    {
        _db.Departments.Update(department);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Department department)
    {
        _db.Departments.Remove(department);
        await _db.SaveChangesAsync();
    }

    public async Task CreateManyAsync(IEnumerable<Department> departments)
    {
        _db.Departments.AddRange(departments);
        await _db.SaveChangesAsync();
    }

    public Task<int> CountByOwnerAsync(int ownerId) =>
        _db.Departments.CountAsync(d => d.OwnerId == ownerId);
}
