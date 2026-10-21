using BudgetBuddy.Kernel;
using BudgetBuddy.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<BBDbContext>(options =>
    options.UseSqlite("Data Source=budget-buddy.db;"));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

using (var db = app.Services
    .GetRequiredService<IDbContextFactory<BBDbContext>>()
    .CreateDbContext())
{
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();