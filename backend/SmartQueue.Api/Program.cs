using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Auth.Domain.Data;

var localEnvPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
var parentEnvPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", ".env"));

if (File.Exists(localEnvPath))
{
    Env.Load(localEnvPath);
}
else if (File.Exists(parentEnvPath))
{
    Env.Load(parentEnvPath);
}

var builder = WebApplication.CreateBuilder(args);

// Register Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();