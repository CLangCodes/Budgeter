using BudgetBuddy.Kernel;
using BudgetBuddy.Data;
using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Transactions;
using HealthChecks.UI.Client;
using BudgetBuddy.Kernel.Messaging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<BBDbContext>(options =>
    options.UseSqlite("Data Source=budget-buddy.db;"));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.Scan(scan => scan.FromAssembliesOf(typeof(Program))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

builder.Services.AddHealthChecks()
    .AddDbContextCheck<BBDbContext>();

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
app.MapHealthChecks("health", new HealthCheckOptions{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

GetTransactionsEndpoint.MapEndpoint(app);
app.Run();