using Application;
using BudgetBuddy;
using BudgetBuddy.Extensions;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddServer();

var app = builder.Build();
app.MapEndpoints();
app.ApplyMigrations();
await app.SeedIdentityRolesAsync();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Mars);
    }).AllowAnonymous();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();
