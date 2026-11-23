using Application;
using BudgetBuddy;
using Infrastructure;
using BudgetBuddy.Extensions;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddServer();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();
app.MapEndpoints();
app.ApplyMigrations();

app.MapHealthChecks("/health", new HealthCheckOptions{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Mars);
    });

    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    if (!await roleManager.RoleExistsAsync(Roles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin));
    }
    if (!await roleManager.RoleExistsAsync(Roles.Member))
    {
        await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Member));
    }
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();