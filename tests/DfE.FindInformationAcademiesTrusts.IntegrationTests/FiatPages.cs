namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

public class FiatPages(EndpointDataSource endpoints)
{
    public static IEnumerable<object[]> AllUnprotected => AlwaysAccessible.Union(NoAccess);

    public static readonly IEnumerable<object[]> AlwaysAccessible =
        new[]
        {
            "accessibility",
            "cookies",
            "privacy"
        }.Select(p => new object[] { p });

    public static readonly IEnumerable<object[]> NoAccess = new[]
    {
        "no-access"
    }.Select(p => new object[] { p });

    public string[] AllEndpoints => endpoints.Endpoints.OfType<RouteEndpoint>()
        .Select(e => e.RoutePattern.RawText ?? string.Empty).ToArray();
}
