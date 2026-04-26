using DevFlow.Projects.Infrastructure.Data;
using DevFlow.Projects.Middleware;
using DevFlow.Projects.Services;
using DevFlow.Shared.Kernel;
using DevFlow.Shared.Kernel.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Add services
builder.Services.AddControllers();

// ✅ Add TenantContext
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<WorkflowService>();
builder.Services.AddScoped<EventService>();

// Add DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Add JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

// ✅ Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Enable Swagger (only in dev)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<TenantMiddleware>();  // ✅ Add TenantMiddleware AFTER authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
