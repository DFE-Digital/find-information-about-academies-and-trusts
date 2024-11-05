using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.WebHost;

public class ApplicationWithMockedServices : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("LocalDevelopment");

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
        var mockTrustService = new Mock<ITrustService>();

        mockTrustService.Setup(t => t.GetTrustSummaryAsync(It.IsAny<string>()))
            .ReturnsAsync(new TrustSummaryServiceModel("1234", "My Trust", "Multi-academy trust", 3));
        mockTrustService.Setup(t => t.GetTrustDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(new TrustDetailsServiceModel("1234", "", "", "", "Multi-academy trust", "", "", null, null));
        mockTrustService.Setup(tp => tp.GetTrustContactsAsync(It.IsAny<string>())).ReturnsAsync(
            new TrustContactsServiceModel(
                new InternalContact("Trust Relationship Manager", "trm@test.com", DateTime.Today, "test@email.com"),
                new InternalContact("SFSO Lead", "sfsol@test.com", DateTime.Today, "test@email.com"),
                new Person("Accounting Officer", "ao@test.com"),
                new Person("Chair Of Trustees", "cot@test.com"),
                new Person("Chief Financial Officer", "cfo@test.com")));

        var mockDataSourceService = new MockDataSourceService();

        services.AddScoped<IAcademyService>(_ => Mock.Of<IAcademyService>());
        services.AddScoped<IDataSourceService>(_ => mockDataSourceService.Object);
        services.AddScoped<IExportService>(_ => Mock.Of<IExportService>());
        services.AddScoped<ITrustService>(_ => mockTrustService.Object);
        services.AddScoped<ITrustSearch>(_ => Mock.Of<ITrustSearch>());
    }
}
