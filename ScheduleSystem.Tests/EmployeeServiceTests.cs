using Moq;
using ScheduleSystem.Api.Models.DTOs.Employee;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Implementations;

namespace ScheduleSystem.Tests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _repo = new();

    private EmployeeService CreateService() => new(_repo.Object);

    [Fact]
    public async Task GetById_WrongOwner_ThrowsUnauthorized()
    {
        _repo.Setup(r => r.GetByIdAsync(1))
             .ReturnsAsync(new Employee { Id = 1, OwnerId = 99, Name = "Alice", Email = "a@a.com" });

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            CreateService().GetByIdAsync(1, ownerId: 1));
    }

    [Fact]
    public async Task GetById_NotFound_ThrowsKeyNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            CreateService().GetByIdAsync(999, ownerId: 1));
    }

    [Fact]
    public async Task GetAll_ReturnsOnlyOwnerEmployees()
    {
        var employees = new List<Employee>
        {
            new() { Id = 1, OwnerId = 1, Name = "Alice", Email = "a@a.com" },
            new() { Id = 2, OwnerId = 1, Name = "Bob",   Email = "b@b.com" }
        };
        _repo.Setup(r => r.GetAllByOwnerAsync(1)).ReturnsAsync(employees);

        var result = await CreateService().GetAllAsync(1);

        Assert.Equal(2, result.Count);
        Assert.All(result, e => Assert.Equal(1, e.OwnerId));
    }

    [Fact]
    public async Task ToggleActive_ActiveEmployee_DeactivatesIt()
    {
        var employee = new Employee { Id = 1, OwnerId = 1, Name = "Carol", Email = "c@c.com", Active = true };
        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);

        await CreateService().ToggleActiveAsync(1, ownerId: 1);

        Assert.False(employee.Active);
        _repo.Verify(r => r.UpdateAsync(employee), Times.Once);
    }

    [Fact]
    public async Task ToggleActive_InactiveEmployee_ActivatesIt()
    {
        var employee = new Employee { Id = 1, OwnerId = 1, Name = "Dave", Email = "d@d.com", Active = false };
        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);

        await CreateService().ToggleActiveAsync(1, ownerId: 1);

        Assert.True(employee.Active);
    }

    [Fact]
    public async Task Delete_WrongOwner_ThrowsUnauthorized()
    {
        _repo.Setup(r => r.GetByIdAsync(1))
             .ReturnsAsync(new Employee { Id = 1, OwnerId = 5, Name = "Eve", Email = "e@e.com" });

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            CreateService().DeleteAsync(1, ownerId: 1));
    }
}
