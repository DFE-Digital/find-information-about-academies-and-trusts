using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

public static class FiatPages
{
    /// <summary>
    /// Routes expected to [AllowAnonymous]
    /// </summary>
    private static string[] ExpectedAlwaysAccessibleRoutes { get; } =
    [
        "accessibility",
        "cookies",
        "privacy",
        "no-access"
    ];

    /// <summary>
    /// Urls with specific parameters
    /// </summary>
    private static string[] AdditionalProtectedUrls { get; } =
    [
        "notfound",
        "search?keywords=trust",
        "search?handler=populateautocomplete&keywords=trust"
    ];

    //The beginning slash is present in the route endpoint data only for health check
    public const string HealthCheckRoute = "/health";

    private const string ValidUid = "2044";

    private static RouteEndpoint[] AllRouteEndpointsInApp { get; } =
        new WebApplicationFactory<Program>()
            .Services.GetService<EndpointDataSource>()!
            .Endpoints.OfType<RouteEndpoint>()
            .ToArray();

    private static string[] AllRoutesInApp { get; } =
        AllRouteEndpointsInApp
            .Select(r => r.RoutePattern.RawText!.ToLower())
            .ToArray();

    private static string[] ExpectedProtectedRoutesInApp { get; } =
        AllRoutesInApp
            .Where(r => !ExpectedAlwaysAccessibleRoutes.Contains(r) && r != HealthCheckRoute)
            .Select(url => url.StartsWith("trusts/") ? $"{url}?uid={ValidUid}" : url)
            .Union(AdditionalProtectedUrls)
            .ToArray();

    private static IEnumerable<object[]> ToXunitMemberData<T>(this IEnumerable<T> collection) where T : notnull
    {
        return collection.Select(item => new object[] { item });
    }

    public static IEnumerable<object[]> ExpectedAlwaysAccessibleRoutesMemberData { get; } =
        ExpectedAlwaysAccessibleRoutes.ToXunitMemberData();

    public static IEnumerable<object[]> ExpectedProtectedRoutesInAppMemberData { get; } =
        ExpectedProtectedRoutesInApp.ToXunitMemberData();
}
