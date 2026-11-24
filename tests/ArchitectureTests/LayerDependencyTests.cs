using System.Reflection;
using NetArchTest.Rules;

namespace ArchitectureTests;

/// <summary>
/// Enforces the Clean Architecture dependency rule: source code dependencies point inward only.
/// SharedKernel &lt;- Domain &lt;- Application &lt;- Infrastructure / BudgetBuddy (Web).
/// </summary>
public class LayerDependencyTests
{
    private const string SharedKernelNamespace = "SharedKernel";
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string WebNamespace = "BudgetBuddy";

    private static readonly Assembly SharedKernelAssembly = typeof(SharedKernel.Result).Assembly;
    private static readonly Assembly DomainAssembly = typeof(Domain.Models.Account).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.DependencyInjection).Assembly;
    private static readonly Assembly WebAssembly = typeof(BudgetBuddy.DependencyInjection).Assembly;

    [Fact]
    public void SharedKernel_ShouldNotDependOn_AnyOtherLayer()
    {
        TestResult result = Types.InAssembly(SharedKernelAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(DomainNamespace, ApplicationNamespace, InfrastructureNamespace, WebNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Domain_ShouldNotDependOn_OuterLayers()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace, WebNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Frameworks()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore")
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Application_ShouldNotDependOn_OuterLayers()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, WebNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Application_ShouldNotDependOn_AspNetCore()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn("Microsoft.AspNetCore")
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOn_Web()
    {
        TestResult result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(WebNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Endpoints_ShouldNotDependOn_Infrastructure()
    {
        TestResult result = Types.InAssembly(WebAssembly)
            .That()
            .ResideInNamespace($"{WebNamespace}.Endpoints")
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    private static void AssertSuccessful(TestResult result) =>
        Assert.True(
            result.IsSuccessful,
            "Offending types: " + string.Join(", ", result.FailingTypeNames ?? []));
}
