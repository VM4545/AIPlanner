using AIPlanner.Data;
using AIPlanner.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/dailyplans", async (DailyPlan plan, ApplicationDbContext db) =>
{
    db.DailyPlans.Add(plan);
    await db.SaveChangesAsync();
    return Results.Created($"/dailyplans/{plan.Id}", plan);
});

app.MapGet("/dailyplans", async (ApplicationDbContext db) =>
{
    var plans = await db.DailyPlans
        .Include(p => p.Tasks)
        .ToListAsync();

    return Results.Ok(plans);
});

app.MapGet("/dailyplans/{id:guid}", async (Guid id, ApplicationDbContext db) =>
{
    var plan = await db.DailyPlans
        .Include(p => p.Tasks)
        .FirstOrDefaultAsync(p => p.Id == id);

    return plan is null ? Results.NotFound() : Results.Ok(plan);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
