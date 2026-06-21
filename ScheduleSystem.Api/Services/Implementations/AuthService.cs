using ScheduleSystem.Api.Helpers;
using ScheduleSystem.Api.Models.DTOs.Auth;
using ScheduleSystem.Api.Models.Entities;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Interfaces;

namespace ScheduleSystem.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IDepartmentRepository _departments;
    private readonly IEmployeeRepository _employees;
    private readonly JwtTokenGenerator _jwt;

    public AuthService(
        IUserRepository users,
        IDepartmentRepository departments,
        IEmployeeRepository employees,
        JwtTokenGenerator jwt)
    {
        _users       = users;
        _departments = departments;
        _employees   = employees;
        _jwt         = jwt;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _users.GetByUsernameAsync(dto.Username)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        return BuildResponse(user);
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (await _users.GetByUsernameAsync(dto.Username) is not null)
            throw new ArgumentException("Username already taken.");

        if (await _users.GetByEmailAsync(dto.Email) is not null)
            throw new ArgumentException("Email already registered.");

        var user = await _users.CreateAsync(new User
        {
            Username     = dto.Username,
            Email        = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Name         = dto.Name,
            Role         = "employee"
        });

        await CreateDefaultDepartmentsAsync(user.Id);

        await _employees.CreateAsync(new Employee
        {
            Name    = dto.Name,
            Email   = dto.Email,
            OwnerId = user.Id
        });

        return BuildResponse(user);
    }

    public async Task<LoginResponseDto> SetupAsync(SetupRequestDto dto)
    {
        if (await _users.AnyAsync())
            throw new InvalidOperationException("Setup has already been completed.");

        var user = await _users.CreateAsync(new User
        {
            Username     = dto.Username,
            Email        = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Name         = dto.Name,
            Role         = "admin"
        });

        await CreateDefaultDepartmentsAsync(user.Id);

        return BuildResponse(user);
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordRequestDto dto)
    {
        var user = await _users.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found.");

        if (!PasswordHasher.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new ArgumentException("Current password is incorrect.");

        user.PasswordHash = PasswordHasher.Hash(dto.NewPassword);
        await _users.UpdateAsync(user);
    }

    public Task<bool> HasUsersAsync() => _users.AnyAsync();

    private LoginResponseDto BuildResponse(User user) => new()
    {
        Token    = _jwt.Generate(user),
        UserId   = user.Id,
        Username = user.Username,
        Name     = user.Name,
        Role     = user.Role,
        Email    = user.Email
    };

    private Task CreateDefaultDepartmentsAsync(int ownerId)
    {
        var names = new[] { "Administration", "Operations", "Human Resources", "Sales", "IT", "Security", "Outlet" };
        var depts  = names.Select(n => new Department { Name = n, OwnerId = ownerId });
        return _departments.CreateManyAsync(depts);
    }
}
