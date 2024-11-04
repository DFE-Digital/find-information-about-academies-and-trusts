using System.Net;
using DfE.FindInformationAcademiesTrusts.IntegrationTests.Base;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

/// <summary>
/// Tests for when the user isn't logged in at all
/// </summary>
public class UnauthenticatedUserTests : BaseIntegrationTest
{
    private readonly HttpClient _client;

    public UnauthenticatedUserTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory]
    [MemberData(nameof(FiatPages.ExpectedAlwaysAccessibleRoutesMemberData), MemberType = typeof(FiatPages))]
    public async Task Renders_page(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Theory]
    [InlineData(FiatPages.HealthCheckRoute)]
    public async Task Renders_health_check_page(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Healthy");
    }

    [Theory]
    [MemberData(nameof(FiatPages.AllExpectedProtectedRoutesInAppMemberData), MemberType = typeof(FiatPages))]
    public async Task Redirects_to_login_when_unauthenticated(string url)
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
