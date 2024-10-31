using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class NoAccessModelTests
{
    private readonly MockHttpContext _mockHttpContext = new();
    private readonly NoAccessModel _sut;
    private readonly TempDataDictionary _tempDataDictionary;

    private const string LocalUrl = "/search";
    private const string ExternalUrl = "https://google.com";

    public NoAccessModelTests()
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(u => u.IsLocalUrl(LocalUrl)).Returns(true);
        mockUrlHelper.Setup(u => u.IsLocalUrl(ExternalUrl)).Returns(false);

        _tempDataDictionary = new TempDataDictionary(_mockHttpContext.Object, Mock.Of<ITempDataProvider>());
        _sut = new NoAccessModel
        {
            PageContext = new PageContext
            {
                HttpContext = _mockHttpContext.Object
            },
            TempData = _tempDataDictionary,
            Url = mockUrlHelper.Object
        };
    }

    [Fact]
    public void OnGet_should_redirect_to_local_return_url_when_user_is_authorised()
    {
        var result = _sut.OnGet(LocalUrl);

        result.Should().BeOfType<LocalRedirectResult>();
        result.As<LocalRedirectResult>().Url.Should().Be(LocalUrl);
    }

    [Theory]
    [InlineData(MockHttpContext.UserAuthState.Authorised)]
    [InlineData(MockHttpContext.UserAuthState.Unauthenticated)]
    [InlineData(MockHttpContext.UserAuthState.Unauthorised)]
    public void OnGet_should_render_page_when_no_return_url_specified(MockHttpContext.UserAuthState authState)
    {
        _mockHttpContext.SetUserTo(authState);

        var result = _sut.OnGet(null);

        result.Should().BeOfType<PageResult>();
    }

    [Theory]
    [InlineData(MockHttpContext.UserAuthState.Authorised)]
    [InlineData(MockHttpContext.UserAuthState.Unauthenticated)]
    [InlineData(MockHttpContext.UserAuthState.Unauthorised)]
    public void OnGet_should_render_page_when_return_url_is_empty(MockHttpContext.UserAuthState authState)
    {
        _mockHttpContext.SetUserTo(authState);

        var result = _sut.OnGet(string.Empty);

        result.Should().BeOfType<PageResult>();
    }

    [Theory]
    [InlineData(MockHttpContext.UserAuthState.Authorised)]
    [InlineData(MockHttpContext.UserAuthState.Unauthenticated)]
    [InlineData(MockHttpContext.UserAuthState.Unauthorised)]
    public void OnGet_should_never_redirect_to_external_return_url(MockHttpContext.UserAuthState authState)
    {
        _mockHttpContext.SetUserTo(authState);

        var result = _sut.OnGet(ExternalUrl);

        result.Should().BeOfType<PageResult>();
        result.Should().NotBeOfType<LocalRedirectResult>();
    }

    [Fact]
    public void OnGet_should_prompt_login_when_user_is_unauthenticated()
    {
        _mockHttpContext.SetUserTo(MockHttpContext.UserAuthState.Unauthenticated);

        var result = _sut.OnGet(LocalUrl);

        using var scope = new AssertionScope();

        result.Should().BeOfType<LocalRedirectResult>();
        result.As<LocalRedirectResult>().Url.Should().Be(LocalUrl);
        _tempDataDictionary.Should().ContainKey("RetryingLogin").WhoseValue.Should().Be("true");
        _mockHttpContext.MockResponseCookies.VerifyNoOtherCalls();
    }

    [Fact]
    public void OnGet_should_retry_login_when_user_is_unauthorised_and_not_retried_login()
    {
        _mockHttpContext.SetUserTo(MockHttpContext.UserAuthState.Unauthorised);

        var result = _sut.OnGet(LocalUrl);

        using var scope = new AssertionScope();

        result.Should().BeOfType<LocalRedirectResult>();
        result.As<LocalRedirectResult>().Url.Should().Be(LocalUrl);
        _tempDataDictionary.Should().ContainKey("RetryingLogin").WhoseValue.Should().Be("true");
        _mockHttpContext.MockResponseCookies.VerifyCookieDeleted(FiatCookies.Login,
            new CookieOptions { HttpOnly = true, IsEssential = true, SameSite = SameSiteMode.Strict, Secure = true });
    }

    [Fact]
    public void OnGet_should_return_no_access_page_when_user_is_unauthorised_and_has_retried_login()
    {
        _mockHttpContext.SetUserTo(MockHttpContext.UserAuthState.Unauthorised);
        _tempDataDictionary.Add("RetryingLogin", "true");

        var result = _sut.OnGet(LocalUrl);

        result.Should().BeOfType<PageResult>();
        _tempDataDictionary.Should().NotContainKey("RetryingLogin");
    }
}
