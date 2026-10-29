using Application;
using BudgetBuddy;
using Infrastructure;
using BudgetBuddy.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddServer();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

// builder.Services.AddHealthChecks()
    // .AddDbContextCheck<BBDbContext>();

var app = builder.Build();
app.MapEndpoints();
// using (var db = app.Services
//     .GetRequiredService<IDbContextFactory<BBDbContext>>()
//     .CreateDbContext())
// {
//     db.Database.Migrate();
// }
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Mars);
    });
}

app.UseHttpsRedirection();
// app.MapHealthChecks("health", new HealthCheckOptions{
//     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
// });

// GetTransactionsEndpoint.MapEndpoint(app);
app.Run();