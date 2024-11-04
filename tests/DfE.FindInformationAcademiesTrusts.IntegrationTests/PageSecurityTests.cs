using DfE.CoreLibs.Testing.Authorization.Validators;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

public class PageSecurityTests
{
    public static IEnumerable<object[]> Endpoints()
    {
        var endpoints = new WebApplicationFactory<Program>()
            .Services.GetService<EndpointDataSource>()!
            .Endpoints.OfType<RouteEndpoint>();

        return endpoints.Select(e => new object[] { e });
    }

    [Theory]
    [MemberData(nameof(Endpoints))]
    public void Check_security(RouteEndpoint route)
    {
        var thing = new PageSecurityValidator(route, true);
        thing.ValidateSinglePageSecurity(route.DisplayName!, "Authorize");

        Assert.Fail(
            "This test currently passes on all endpoints, including those which are not secured. Adding failure to ensure that we fix it!");
    }
}
