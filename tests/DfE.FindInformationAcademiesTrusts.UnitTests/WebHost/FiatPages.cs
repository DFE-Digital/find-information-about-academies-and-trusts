using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.WebHost;

public static class FiatPages
{
    //The beginning slash is present in the route endpoint data only for health check
    public const string HealthCheckRoute = "/health";

    /// <summary>
    /// Routes expected to [AllowAnonymous]
    /// </summary>
    private static string[] ExpectedAllowAnonymousRoutes { get; } =
    [
        "accessibility",
        "cookies",
        "privacy",
        "no-access"
    ];

    private static string[] AllEndpointsInApp { get; } =
        new WebApplicationFactory<Program>()
            .Services.GetService<EndpointDataSource>()!
            .Endpoints.OfType<RouteEndpoint>()
            .Select(r => r.RoutePattern.RawText!.ToLower())
            .ToArray();

    private static string[] ExpectedProtectedRoutesInApp { get; } =
        AllEndpointsInApp
            .Where(IsNotAllowAnonymousRoute)
            .ToArray();

    private static bool IsNotAllowAnonymousRoute(string r)
    {
        return !ExpectedAllowAnonymousRoutes.Contains(r) && r != HealthCheckRoute;
    }

    private static IEnumerable<object[]> ToXunitMemberData<T>(this IEnumerable<T> collection) where T : notnull
    {
        return collection.Select(item => new object[] { item });
    }

    public static IEnumerable<object[]> ExpectedAlwaysAccessibleRoutesMemberData { get; } =
        ExpectedAllowAnonymousRoutes.ToXunitMemberData();

    public static IEnumerable<object[]> ExpectedProtectedRoutesInAppMemberData { get; } =
        ExpectedProtectedRoutesInApp.ToXunitMemberData();
}
