using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

public static class FiatPages
{
    private static string[] ExpectedAlwaysAccessibleRoutes { get; } =
    [
        "accessibility",
        "cookies",
        "privacy",
        "no-access"
    ];

    //The beginning slash is present in the route endpoint data only for health check
    public const string HealthCheckRoute = "/health";

    private static RouteEndpoint[] AllRouteEndpointsInApp { get; } =
        new WebApplicationFactory<Program>()
            .Services.GetService<EndpointDataSource>()!
            .Endpoints.OfType<RouteEndpoint>()
            .ToArray();

    private static string[] AllRoutesInApp { get; } =
        AllRouteEndpointsInApp
            .Select(r => r.RoutePattern.RawText!.ToLower())
            .ToArray();

    private static string[] AllExpectedProtectedRoutesInApp { get; } =
        AllRoutesInApp
            .Where(r => !ExpectedAlwaysAccessibleRoutes.Contains(r) && r != HealthCheckRoute)
            .ToArray();

    private static IEnumerable<object[]> ToXunitMemberData<T>(this IEnumerable<T> collection) where T : notnull
    {
        return collection.Select(item => new object[] { item });
    }

    public static IEnumerable<object[]> ExpectedAlwaysAccessibleRoutesMemberData { get; } =
        ExpectedAlwaysAccessibleRoutes.ToXunitMemberData();

    public static IEnumerable<object[]> AllExpectedProtectedRoutesInAppMemberData { get; } =
        AllExpectedProtectedRoutesInApp.ToXunitMemberData();
}
