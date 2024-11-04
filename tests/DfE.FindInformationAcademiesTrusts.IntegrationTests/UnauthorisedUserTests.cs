using System.Net;
using DfE.FindInformationAcademiesTrusts.IntegrationTests.AuthenticationHandlers;
using DfE.FindInformationAcademiesTrusts.IntegrationTests.Base;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests;

/// <summary>
/// Tests for when the user is authenticated but doesn't have the correct role
/// </summary>
public class UnauthorisedUserTests : BaseIntegrationTest
{
    private readonly HttpClient _client;

    public UnauthorisedUserTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    //Override authentication to simulate unauthorised user
                    services.AddAuthentication(UnauthorisedUserAuthenticationHandler.AuthenticationScheme)
                        .AddScheme<AuthenticationSchemeOptions, UnauthorisedUserAuthenticationHandler>(
                            UnauthorisedUserAuthenticationHandler.AuthenticationScheme,
                            options => { options.ForwardForbid = CookieAuthenticationDefaults.AuthenticationScheme; }
                        );
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
    }

    [Theory]
    [MemberData(nameof(FiatPages.ExpectedAlwaysAccessibleRoutesMemberData), MemberType = typeof(FiatPages))]
    public async Task Unprotected_page_renders_when_unauthorised(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Theory]
    [InlineData(FiatPages.HealthCheckRoute)]
    public async Task Health_check_page_renders_when_unauthorised(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Healthy");
    }

    [Theory]
    [MemberData(nameof(FiatPages.ExpectedProtectedRoutesInAppMemberData), MemberType = typeof(FiatPages))]
    [InlineData("notfound")]
    public async Task Protected_pages_redirect_to_no_access_when_unauthorised(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.Host.Should().Be("localhost");
        response.Headers.Location!.Segments.Should().Contain("no-access");
        response.Headers.Location!.Query.Should().Be($"?ReturnUrl={WebUtility.UrlEncode($"/{url}")}");
    }
}
