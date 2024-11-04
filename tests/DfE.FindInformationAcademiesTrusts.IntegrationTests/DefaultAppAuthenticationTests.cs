using System.Net;
using DfE.FindInformationAcademiesTrusts.IntegrationTests.Base;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

/// <summary>
/// Tests with unmodified authentication behaviour to simulate when the user isn't logged in at all
/// </summary>
public class DefaultAppAuthenticationTests : BaseIntegrationTest
{
    private readonly HttpClient _client;

    public DefaultAppAuthenticationTests(ApplicationWithMockedServices factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory]
    [MemberData(nameof(FiatPages.ExpectedAlwaysAccessibleRoutesMemberData), MemberType = typeof(FiatPages))]
    public async Task Unprotected_page_renders_when_unauthenticated(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Fact]
    public async Task Health_check_page_renders_when_unauthenticated()
    {
        var response = await _client.GetAsync(FiatPages.HealthCheckRoute);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Healthy");
    }

    [Theory]
    [MemberData(nameof(FiatPages.ExpectedProtectedRoutesInAppMemberData), MemberType = typeof(FiatPages))]
    [InlineData("notfound")]
    public async Task Protected_pages_redirect_to_login_when_unauthenticated(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.Host.Should().Be("login.microsoftonline.com");
        response.Headers.Location!.Segments.Should().Contain("authorize");
        response.Headers.Location!.Query.Should().Contain("redirect_uri=http%3A%2F%2Flocalhost%2Fsignin-oidc");
    }
}
