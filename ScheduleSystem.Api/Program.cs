using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ScheduleSystem.Api.Data;
using ScheduleSystem.Api.Helpers;
using ScheduleSystem.Api.Middleware;
using ScheduleSystem.Api.Repositories.Implementations;
using ScheduleSystem.Api.Repositories.Interfaces;
using ScheduleSystem.Api.Services.Implementations;
using ScheduleSystem.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")));

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? Environment.GetEnvironmentVariable("JWT_KEY")
    ?? throw new InvalidOperationException("JWT Key not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://urielmju.github.io")
              .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
              .WithHeaders("Authorization", "Content-Type");
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScheduleSystem API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Helpers
builder.Services.AddScoped<JwtTokenGenerator>();

// Repositories
builder.Services.AddScoped<IUserRepository,         UserRepository>();
builder.Services.AddScoped<IEmployeeRepository,     EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository,   DepartmentRepository>();
builder.Services.AddScoped<IShiftRepository,        ShiftRepository>();
builder.Services.AddScoped<IPlanRepository,         PlanRepository>();
builder.Services.AddScoped<IScheduleRepository,     ScheduleRepository>();
builder.Services.AddScoped<IVacationRepository,     VacationRepository>();
builder.Services.AddScoped<IDaysOffRepository,      DaysOffRepository>();
builder.Services.AddScoped<ICaptainRepository,      CaptainRepository>();
builder.Services.AddScoped<IHistoryRepository,      HistoryRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Services
builder.Services.AddScoped<IAuthService,         AuthService>();
builder.Services.AddScoped<IEmployeeService,     EmployeeService>();
builder.Services.AddScoped<IDepartmentService,   DepartmentService>();
builder.Services.AddScoped<IShiftService,        ShiftService>();
builder.Services.AddScoped<IPlanService,         PlanService>();
builder.Services.AddScoped<IScheduleService,     ScheduleService>();
builder.Services.AddScoped<IVacationService,     VacationService>();
builder.Services.AddScoped<IDaysOffService,      DaysOffService>();
builder.Services.AddScoped<ICaptainService,      CaptainService>();
builder.Services.AddScoped<IHistoryService,      HistoryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IReportService,       ReportService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
