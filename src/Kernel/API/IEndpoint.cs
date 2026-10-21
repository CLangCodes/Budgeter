namespace BudgetBuddy.Kernel.Api;

internal interface IEndpoint
{
    static abstract void MapEndpoint(IEndpointRouteBuilder app);
}