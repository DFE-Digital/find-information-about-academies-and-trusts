using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

/// <summary>
/// Tests for when the user isn't logged in at all
/// </summary>
public class UnauthenticatedUserTests : IClassFixture<WebApplicationFactory<Program>>
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
    [InlineData("/accessibility")]
    [InlineData("/cookies")]
    [InlineData("/no-access")]
    [InlineData("/privacy")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/trusts/details?uid=2044")]
    [InlineData("/search?keywords=trust")]
    [InlineData("/trusts/academies/details?uid=2044&handler=export")]
    [InlineData("/signin-oidc")]
    [InlineData("/notfound")]
    [InlineData("/error")]
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
