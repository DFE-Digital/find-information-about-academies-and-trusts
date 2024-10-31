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
/// Tests for when the user is authenticated and has the authorised role
/// </summary>
public class AuthorisedUserTests : BaseIntegrationTest
{
    private readonly HttpClient _client;

    public AuthorisedUserTests(WebApplicationFactory<Program> factory)
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
    [InlineData("/")]
    [InlineData("/trusts/details?uid=2044")]
    [InlineData("/search?keywords=trust")]
    [InlineData("/error")]
    [InlineData("/accessibility")]
    [InlineData("/cookies")]
    [InlineData("/privacy")]
    public async Task Renders_page(string url)
    {
        var response = await _client.GetAsync(url);

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Fact]
    public async Task Redirects_to_not_found()
    {
        var response = await _client.GetAsync("/notfound");

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().Contain("Find information about academies and trusts");
    }

    [Fact]
    public async Task Returns_data_download()
    {
        var response = await _client.GetAsync("/trusts/academies/details?uid=2044&handler=export");

        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should()
            .Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        response.Content.Headers.ContentDisposition.Should().NotBeNull();
        response.Content.Headers.ContentDisposition!.DispositionType.Should().Be("attachment");

        var pageContent = await response.Content.ReadAsStringAsync();
        pageContent.Should().NotBeNull();
    }
}
