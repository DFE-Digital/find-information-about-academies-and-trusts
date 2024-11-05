using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.WebHost;

public class ApplicationWithMockedServices : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            RemoveFiatServices(services);
            AddMockedFiatServices(services);
        });
    }

    private void RemoveFiatServices(IServiceCollection services)
    {
        services.RemoveAll<IAcademyService>();
        services.RemoveAll<IDataSourceService>();
        services.RemoveAll<IExportService>();
        services.RemoveAll<ITrustService>();
        services.RemoveAll<ITrustSearch>();
    }

    private void AddMockedFiatServices(IServiceCollection services)
    {
        services.AddScoped<IAcademyService>(_ => Mock.Of<IAcademyService>());
        services.AddScoped<IDataSourceService>(_ => Mock.Of<IDataSourceService>());
        services.AddScoped<IExportService>(_ => Mock.Of<IExportService>());
        services.AddScoped<ITrustService>(_ => Mock.Of<ITrustService>());
        services.AddScoped<ITrustSearch>(_ => Mock.Of<ITrustSearch>());
    }
}
