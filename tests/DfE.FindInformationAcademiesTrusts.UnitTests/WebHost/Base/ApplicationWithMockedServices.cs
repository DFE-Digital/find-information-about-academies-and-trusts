using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.WebHost.Base;

public class ApplicationWithMockedServices : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("LocalDevelopment");

        builder.ConfigureTestServices(services =>
        {
            RemoveFiatTrustSearch(services);
            AddMockedFiatTrustSearch(services);

            RemoveFiatServices(services);
            AddMockedFiatServices(services);
        });
    }

    private void RemoveFiatTrustSearch(IServiceCollection services)
    {
        services.RemoveAll<ITrustSearch>();
    }

    private void AddMockedFiatTrustSearch(IServiceCollection services)
    {
        var mockTrustSearch = new Mock<ITrustSearch>();
        mockTrustSearch.Setup(s => s.SearchAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(PaginatedList<TrustSearchEntry>.Empty());

        services.AddScoped<ITrustSearch>(_ => mockTrustSearch.Object);
    }

    private void RemoveFiatServices(IServiceCollection services)
    {
        foreach (var serviceInterface in FiatServiceInterfaces)
        {
            services.RemoveAll(serviceInterface);
        }
    }

    private void AddMockedFiatServices(IServiceCollection services)
    {
        foreach (var serviceInterface in FiatServiceInterfaces)
        {
            var serviceMock = (Mock)Activator.CreateInstance(typeof(Mock<>).MakeGenericType(serviceInterface))!;
            serviceMock.DefaultValueProvider = new FiatObjectDefaultValueProvider();

            services.AddScoped(serviceInterface, _ => serviceMock.Object);
        }
    }

    private static IEnumerable<Type> FiatServiceInterfaces { get; } = GetFiatServiceInterfaces();

    private static IEnumerable<Type> GetFiatServiceInterfaces()
    {
        var assembly = typeof(Program).Assembly;
        var serviceInterfaces = assembly.GetTypes()
            .Where(t => t is
            {
                IsInterface: true,
                IsPublic: true,
                Namespace: not null
            } && t.Namespace.StartsWith("DfE.FindInformationAcademiesTrusts.Services"));
        return serviceInterfaces;
    }
}
