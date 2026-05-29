using System.Text;
using AIManHua.Domain.Interfaces;
using AIManHua.Infrastructure.Data;
using AIManHua.Infrastructure.Repositories;
using AIManHua.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

var mysqlConn = builder.Configuration.GetConnectionString("mysql")
    ?? throw new InvalidOperationException("MySQL connection string 'mysql' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(mysqlConn));

// Snowflake ID generator (WorkerId=1 for API service)
builder.Services.AddSingleton(new SnowflakeIdGenerator(workerId: 1));

// JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = jwtSection["Secret"]
    ?? throw new InvalidOperationException("JWT Secret is not configured");
var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"] ?? "AIManHua",
            ValidAudience = jwtSection["Audience"] ?? "AIManHua",
            IssuerSigningKey = jwtKey
        };
    });

builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IComicTaskRepository, AIManHua.Infrastructure.Repositories.ComicTaskRepository>();

// Services
builder.Services.AddSingleton<JwtService>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    for (var i = 0; ; i++)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex) when (i < 5)
        {
            app.Logger.LogWarning("DB migration attempt {Attempt} failed: {Error}. Retrying in 3s...", i + 1, ex.Message);
            Thread.Sleep(3000);
        }
    }
}

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
