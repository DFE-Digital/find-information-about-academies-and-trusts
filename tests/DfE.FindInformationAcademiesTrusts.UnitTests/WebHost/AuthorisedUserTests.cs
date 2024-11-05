using System.Net;
using DfE.FindInformationAcademiesTrusts.UnitTests.WebHost.AuthenticationHandlers;
using DfE.FindInformationAcademiesTrusts.UnitTests.WebHost.Base;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.WebHost;

/// <summary>
/// Tests for when the user is authenticated and has the authorised role
/// </summary>
public class AuthorisedUserTests : BaseWebHostTest
{
    private readonly HttpClient _client;

    public AuthorisedUserTests(ApplicationWithMockedServices factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(AuthorisedUserAuthenticationHandler.AuthenticationScheme)
                        .AddScheme<AuthenticationSchemeOptions, AuthorisedUserAuthenticationHandler>(
                            AuthorisedUserAuthenticationHandler.AuthenticationScheme,
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
    [MemberData(nameof(FiatPages.ExpectedProtectedRoutesInAppMemberData), MemberType = typeof(FiatPages))]
    [MemberData(nameof(FiatPages.ExpectedAlwaysAccessibleRoutesMemberData), MemberType = typeof(FiatPages))]
    public async Task Protected_page_renders_when_authorised(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Fact]
    public async Task Health_check_page_renders_when_unauthorised()
    {
        var response = await _client.GetAsync(FiatPages.HealthCheckRoute);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Healthy");
    }

    [Fact]
    public async Task Not_found_page_renders_when_authorised()
    {
        var response = await _client.GetAsync("/notfound");

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Fact]
    public async Task No_access_redirects_to_return_url()
    {
        var response = await _client.GetAsync($"no-access?ReturnUrl={WebUtility.UrlEncode("/search")}");

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.OriginalString.Should().Be("/search");
    }
}
