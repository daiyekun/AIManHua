using AIManHua.AgentService.Agents;
using AIManHua.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

var mysqlConn = builder.Configuration.GetConnectionString("mysql")
    ?? throw new InvalidOperationException("MySQL connection string 'mysql' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(mysqlConn));

builder.Services.AddKernel();
builder.Services.AddSingleton<ComicGenAgent>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
