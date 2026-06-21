using Moq;
using ScheduleSystem.Api.Models.DTOs.Schedule;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Implementations;

namespace ScheduleSystem.Tests;

public class ScheduleServiceTests
{
    private readonly Mock<IScheduleRepository> _repo = new();

    private ScheduleService CreateService() => new(_repo.Object);

    [Fact]
    public async Task Create_WorkSchedule_DuplicateThrowsException()
    {
        var dto = new CreateScheduleDto
        {
            EmployeeId = 1,
            ShiftId    = 1,
            Date       = new DateOnly(2025, 1, 15),
            Type       = "work",
            PlanId     = 1
        };

        _repo.Setup(r => r.ExistsWorkScheduleAsync(1, dto.Date, 1)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateService().CreateAsync(dto, ownerId: 1, createdBy: 1));
    }

    [Fact]
    public async Task Create_FreeSchedule_DoesNotCheckDuplicate()
    {
        var dto = new CreateScheduleDto
        {
            EmployeeId = 1,
            Date       = new DateOnly(2025, 1, 15),
            Type       = "free",
            PlanId     = 1
        };

        var saved = new Schedule
        {
            Id         = 10,
            EmployeeId = 1,
            Date       = dto.Date,
            Type       = "free",
            PlanId     = 1,
            CreatedBy  = 1,
            OwnerId    = 1,
            Employee   = new Employee { Name = "John" }
        };

        _repo.Setup(r => r.CreateAsync(It.IsAny<Schedule>())).ReturnsAsync(saved);
        _repo.Setup(r => r.GetByDateAsync(1, dto.Date)).ReturnsAsync(new List<Schedule> { saved });

        var result = await CreateService().CreateAsync(dto, ownerId: 1, createdBy: 1);

        _repo.Verify(r => r.ExistsWorkScheduleAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<int>()), Times.Never);
        Assert.Equal("free", result.Type);
    }

    [Fact]
    public async Task Create_WorkSchedule_NoDuplicate_Succeeds()
    {
        var dto = new CreateScheduleDto
        {
            EmployeeId = 2,
            ShiftId    = 1,
            Date       = new DateOnly(2025, 2, 10),
            Type       = "work",
            PlanId     = 2
        };

        var saved = new Schedule
        {
            Id         = 5,
            EmployeeId = 2,
            ShiftId    = 1,
            Date       = dto.Date,
            Type       = "work",
            PlanId     = 2,
            CreatedBy  = 1,
            OwnerId    = 1,
            Employee   = new Employee { Name = "Jane" },
            Shift      = new Shift { Name = "8am - 5pm", Color = "#F6AD55" }
        };

        _repo.Setup(r => r.ExistsWorkScheduleAsync(2, dto.Date, 2)).ReturnsAsync(false);
        _repo.Setup(r => r.CreateAsync(It.IsAny<Schedule>())).ReturnsAsync(saved);
        _repo.Setup(r => r.GetByDateAsync(1, dto.Date)).ReturnsAsync(new List<Schedule> { saved });

        var result = await CreateService().CreateAsync(dto, ownerId: 1, createdBy: 1);

        Assert.Equal("work", result.Type);
        Assert.Equal("Jane", result.EmployeeName);
    }
}
