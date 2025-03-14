using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class CookiesModelTests
{
    private readonly MockHttpContext _mockHttpContext;
    private readonly CookiesModel _sut;
    private readonly TempDataDictionary _tempData;

    public CookiesModelTests()
    {
        IHttpContextAccessor mockHttpAccessor = Substitute.For<IHttpContextAccessor>();
        _mockHttpContext = new MockHttpContext();
        mockHttpAccessor.HttpContext.Returns(_mockHttpContext.Object);
        var actionContext = new ActionContext(_mockHttpContext.Object, new RouteData(), new ActionDescriptor());
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        _tempData = new TempDataDictionary(_mockHttpContext.Object, tempDataProvider);
        _sut = new CookiesModel(mockHttpAccessor)
        {
            TempData = _tempData,
            Url = new UrlHelper(actionContext)
        };
    }

    //Check return path is sanitised
    [Theory]
    [InlineData("/")]
    [InlineData("/page")]
    [InlineData("/index")]
    [InlineData("/search?keywords=\"test\"")]
    public void ReturnPath_should_not_change_when_set_to_valid_value(string path)
    {
        _sut.ReturnPath = path;
        _sut.ReturnPath.Should().Be(path);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("test")]
    [InlineData("https://www.gov.uk")]
    [InlineData(null)]
    public void ReturnPath_should_default_to_home_when_set_to_invalid_value(string? path)
    {
        _sut.ReturnPath = path;
        _sut.ReturnPath.Should().Be("/");
    }

    [Fact]
    public void ReturnPath_should_be_null_if_not_set()
    {
        _sut.ReturnPath.Should().BeNull();
    }

    //Check Consent is set correctly

    [Theory]
    [InlineData(null)]
    [InlineData(true)]
    [InlineData(false)]
    public void OnGet_should_set_consent_to_current_cookie_value(bool? consentCookieValue)
    {
        _mockHttpContext.MockRequestCookies.SetupConsentCookie(consentCookieValue);
        _sut.OnGet();
        _sut.Consent.Should().Be(consentCookieValue);
    }

    [Theory]
    [InlineData(true, null)]
    [InlineData(false, null)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void OnPost_should_not_change_consent_when_it_is_provided(bool consent, bool? accepted)
    {
        _mockHttpContext.MockRequestCookies.SetupConsentCookie(accepted);
        _sut.Consent = consent;
        _sut.OnPost();
        _sut.Consent.Should().Be(consent);
    }

    [Theory]
    [InlineData(true, null)]
    [InlineData(false, null)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void OnPostFromBanner_should_not_change_consent_when_it_is_provided(bool consent, bool? accepted)
    {
        _mockHttpContext.MockRequestCookies.SetupConsentCookie(accepted);
        _sut.Consent = consent;
        _sut.OnPostFromBanner();
        _sut.Consent.Should().Be(consent);
    }

    [Fact]
    public void OnGet_should_not_set_consent_when_it_not_provided_and_the_cookie_has_not_been_set()
    {
        _sut.OnGet();
        _sut.Consent.Should().BeNull();
    }

    [Fact]
    public void OnPost_should_not_set_consent_when_it_not_provided_and_the_cookie_has_not_been_set()
    {
        _sut.OnPost();
        _sut.Consent.Should().BeNull();
    }

    [Fact]
    public void OnGet_DisplayCookieChangedMessageOnCookiesPage_should_always_be_false()
    {
        _sut.OnGet();
        _sut.DisplayCookieChangedMessageOnCookiesPage.Should().BeFalse();
    }

    [Fact]
    public void OnPost_DisplayCookieChangedMessageOnCookiesPage_should_always_be_true()
    {
        _sut.OnPost();
        _sut.DisplayCookieChangedMessageOnCookiesPage.Should().BeTrue();
    }

    [Fact]
    public void OnGet_returns_Cookies_page()
    {
        var result = _sut.OnGet();
        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public void OnPost_returns_Cookies_page()
    {
        var result = _sut.OnPost();
        result.Should().BeOfType<PageResult>();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/page")]
    [InlineData("/index")]
    [InlineData("/search?keywords=\"test\"")]
    public void OnPostFromBanner_redirects_to_return_url(string redirectUrl)
    {
        _sut.ReturnPath = redirectUrl;

        var result = _sut.OnPostFromBanner();

        result.Should().BeOfType<LocalRedirectResult>()
            .Which.Url.Should().Be(redirectUrl);
    }

    [Fact]
    public void OnPostFromBanner_redirects_to_home_when_ReturnPath_not_set()
    {
        var result = _sut.OnPostFromBanner();

        result.Should().BeOfType<LocalRedirectResult>()
            .Which.Url.Should().Be("/");
    }

    // Consent cookie
    [Theory]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    public void OnPost_adds_consent_cookie_when_consent_is_given(bool consent, string value)
    {
        _sut.Consent = consent;
        _sut.OnPost();
        _mockHttpContext.MockResponseCookies.VerifySecureCookieAdded(FiatCookies.CookieConsent, value);
    }

    [Theory]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    public void OnPostFromBanner_adds_consent_cookie_when_consent_is_given(bool consent, string value)
    {
        _sut.Consent = consent;
        _sut.OnPostFromBanner();
        _mockHttpContext.MockResponseCookies.VerifySecureCookieAdded(FiatCookies.CookieConsent, value);
    }

    // (Cookie appended Delete called if needed/TempData is not null)
    [Theory]
    [InlineData("ai_session")]
    [InlineData("ai_user")]
    [InlineData("_ga")]
    [InlineData("_gid")]
    public void OnPost_removes_optional_cookie_when_consent_is_false(string cookieName)
    {
        _mockHttpContext.MockRequestCookies.SetupOptionalCookies();
        _sut.Consent = false;
        _sut.OnPost();
        _mockHttpContext.MockResponseCookies.VerifyCookieDeleted(cookieName);
    }

    [Theory]
    [InlineData("ai_session")]
    [InlineData("ai_user")]
    [InlineData("_ga")]
    [InlineData("_gid")]
    public void OnPostFromBanner_removes_optional_cookie_when_consent_is_false(string cookieName)
    {
        _mockHttpContext.MockRequestCookies.SetupOptionalCookies();
        _sut.Consent = false;
        _sut.OnPostFromBanner();
        _mockHttpContext.MockResponseCookies.VerifyCookieDeleted(cookieName);
    }

    [Fact]
    public void OnGet_does_not_remove_cookies_when_consent_is_true()
    {
        _mockHttpContext.MockRequestCookies.SetupOptionalCookies();
        _mockHttpContext.MockRequestCookies.SetupAcceptedCookie();
        _sut.Consent = true;
        _sut.OnGet();
        _mockHttpContext.MockResponseCookies.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnPost_does_not_remove_cookies_when_consent_is_true()
    {
        _mockHttpContext.MockRequestCookies.SetupOptionalCookies();
        _mockHttpContext.MockRequestCookies.SetupAcceptedCookie();
        _sut.Consent = true;
        _sut.OnPost();
        _mockHttpContext.MockResponseCookies.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnPostFromBanner_does_not_remove_cookies_when_consent_is_true()
    {
        _mockHttpContext.MockRequestCookies.SetupOptionalCookies();
        _mockHttpContext.MockRequestCookies.SetupAcceptedCookie();
        _sut.Consent = true;
        _sut.OnPostFromBanner();
        _mockHttpContext.MockResponseCookies.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnGet_does_not_remove_cookies_when_there_are_no_optional_cookies_to_remove()
    {
        _mockHttpContext.MockRequestCookies.SetupRejectedCookie();
        _sut.Consent = false;
        _sut.OnGet();
        _mockHttpContext.MockResponseCookies.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnPost_does_not_remove_cookies_when_there_are_no_optional_cookies_to_remove()
    {
        _mockHttpContext.MockRequestCookies.SetupRejectedCookie();
        _sut.Consent = false;
        _sut.OnPost();
        _mockHttpContext.MockResponseCookies.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnPostFromBanner_does_not_remove_cookies_when_there_are_no_optional_cookies_to_remove()
    {
        _mockHttpContext.MockRequestCookies.SetupRejectedCookie();
        _sut.Consent = false;
        _sut.OnPostFromBanner();
        _mockHttpContext.MockResponseCookies.VerifyNoCookiesDeleted();
    }

    //TempData
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnPost_sets_tempdata_cookie_changed_when_consent_is_given(bool consent)
    {
        _sut.Consent = consent;
        _sut.OnPost();
        Assert.NotNull(_tempData[CookiesHelper.CookieChangedTempDataName]);
        _tempData[CookiesHelper.CookieChangedTempDataName].As<bool>().Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnPostFromBanner_sets_tempdata_cookie_changed_when_consent_is_given(bool consent)
    {
        _sut.Consent = consent;
        _sut.OnPostFromBanner();
        Assert.NotNull(_tempData[CookiesHelper.CookieChangedTempDataName]);
        _tempData[CookiesHelper.CookieChangedTempDataName].As<bool>().Should().BeTrue();
    }

    [Fact]
    public void OnGet_does_not_set_tempdata_cookie_changed_when_consent_is_not_given()
    {
        _sut.OnGet();
        _tempData[CookiesHelper.CookieChangedTempDataName].Should().BeNull();
    }

    [Fact]
    public void OnPost_does_not_set_tempdata_cookie_changed_when_consent_is_not_given()
    {
        _sut.OnPost();
        _tempData[CookiesHelper.CookieChangedTempDataName].Should().BeNull();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(null)]
    public void OnGet_does_not_set_tempdata_cookie_deleted_when_consent_is_true_not_given(bool? consent)
    {
        _sut.Consent = consent;
        _sut.OnGet();
        _tempData[CookiesHelper.DeleteCookieTempDataName].Should().BeNull();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(null)]
    public void OnPost_does_not_set_tempdata_cookie_deleted_when_consent_is_true_not_given(bool? consent)
    {
        _sut.Consent = consent;
        _sut.OnPost();
        _tempData[CookiesHelper.DeleteCookieTempDataName].Should().BeNull();
    }

    [Fact]
    public void OnPostFromBanner_does_not_set_tempdata_cookie_deleted_when_consent_is_true()
    {
        _sut.Consent = true;
        _sut.OnPostFromBanner();
        _tempData[CookiesHelper.DeleteCookieTempDataName].Should().BeNull();
    }

    [Fact]
    public void OnPost_sets_tempdata_cookie_deleted_when_consent_is_false()
    {
        _sut.Consent = false;
        _sut.OnPost();
        _tempData[CookiesHelper.DeleteCookieTempDataName].Should().NotBeNull();
        _tempData[CookiesHelper.DeleteCookieTempDataName].As<bool>().Should().BeTrue();
    }

    [Fact]
    public void OnPostFromBanner_sets_tempdata_cookie_deleted_when_consent_is_false()
    {
        _sut.Consent = false;
        _sut.OnPostFromBanner();
        _tempData[CookiesHelper.DeleteCookieTempDataName].Should().NotBeNull();
        _tempData[CookiesHelper.DeleteCookieTempDataName].As<bool>().Should().BeTrue();
    }
}
