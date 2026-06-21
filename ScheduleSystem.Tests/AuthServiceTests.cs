using Microsoft.Extensions.Configuration;
using Moq;
using ScheduleSystem.Api.Helpers;
using ScheduleSystem.Api.Models.DTOs.Auth;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Implementations;

namespace ScheduleSystem.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository>       _users       = new();
    private readonly Mock<IDepartmentRepository> _departments = new();
    private readonly Mock<IEmployeeRepository>   _employees   = new();
    private readonly JwtTokenGenerator           _jwt;

    public AuthServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"]      = "test-secret-key-that-is-long-enough-32-chars!!",
                ["Jwt:Issuer"]   = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            })
            .Build();
        _jwt = new JwtTokenGenerator(config);
    }

    private AuthService CreateService() =>
        new(_users.Object, _departments.Object, _employees.Object, _jwt);

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        var hash = PasswordHasher.Hash("password123");
        _users.Setup(r => r.GetByUsernameAsync("admin"))
              .ReturnsAsync(new User { Id = 1, Username = "admin", Name = "Admin", Role = "admin", PasswordHash = hash, Email = "a@a.com" });

        var result = await CreateService().LoginAsync(new LoginRequestDto { Username = "admin", Password = "password123" });

        Assert.NotNull(result.Token);
        Assert.Equal("admin", result.Username);
    }

    [Fact]
    public async Task Login_InvalidPassword_ThrowsUnauthorized()
    {
        var hash = PasswordHasher.Hash("correctpassword");
        _users.Setup(r => r.GetByUsernameAsync("admin"))
              .ReturnsAsync(new User { Id = 1, Username = "admin", Name = "Admin", Role = "admin", PasswordHash = hash, Email = "a@a.com" });

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            CreateService().LoginAsync(new LoginRequestDto { Username = "admin", Password = "wrongpassword" }));
    }

    [Fact]
    public async Task Login_UserNotFound_ThrowsUnauthorized()
    {
        _users.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            CreateService().LoginAsync(new LoginRequestDto { Username = "nobody", Password = "x" }));
    }

    [Fact]
    public async Task Register_DuplicateUsername_ThrowsArgumentException()
    {
        _users.Setup(r => r.GetByUsernameAsync("existing")).ReturnsAsync(new User());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            CreateService().RegisterAsync(new RegisterRequestDto
            {
                Username = "existing", Email = "e@e.com", Password = "pass123", Name = "Test"
            }));
    }

    [Fact]
    public async Task Register_DuplicateEmail_ThrowsArgumentException()
    {
        _users.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _users.Setup(r => r.GetByEmailAsync("taken@e.com")).ReturnsAsync(new User());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            CreateService().RegisterAsync(new RegisterRequestDto
            {
                Username = "newuser", Email = "taken@e.com", Password = "pass123", Name = "Test"
            }));
    }

    [Fact]
    public async Task Setup_WhenUsersExist_ThrowsInvalidOperation()
    {
        _users.Setup(r => r.AnyAsync()).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateService().SetupAsync(new SetupRequestDto
            {
                Username = "admin", Email = "a@a.com", Password = "password123", Name = "Admin"
            }));
    }
}
