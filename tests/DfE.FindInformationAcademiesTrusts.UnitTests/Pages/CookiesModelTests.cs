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
        Mock<IHttpContextAccessor> mockHttpAccessor = new();
        _mockHttpContext = new MockHttpContext();
        mockHttpAccessor.Setup(m => m.HttpContext).Returns(_mockHttpContext.Object);
        var actionContext = new ActionContext(_mockHttpContext.Object, new RouteData(), new ActionDescriptor());
        _tempData = new TempDataDictionary(_mockHttpContext.Object, Mock.Of<ITempDataProvider>());
        _sut = new CookiesModel(mockHttpAccessor.Object)
        {
            TempData = _tempData,
            Url = new UrlHelper(actionContext)
        };
    }

    //Check return path is set correctly
    //Return path set correctly when valid local path
    [Theory]
    [InlineData("/")]
    [InlineData("/page")]
    [InlineData("/index")]
    [InlineData("/search?keywords=\"test\"")]
    public void OnGet_should_keep_return_path_when_it_is_valid(string path)
    {
        _sut.ReturnPath = path;
        _sut.OnGet();
        _sut.ReturnPath.Should().Be(path);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/page")]
    [InlineData("/index")]
    [InlineData("/search?keywords=\"test\"")]
    public void OnPost_should_keep_return_path_when_it_is_valid(string path)
    {
        _sut.ReturnPath = path;
        _sut.OnPost();
        _sut.ReturnPath.Should().Be(path);
    }

    //Return path set to / when invalid
    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("https://www.gov.uk")]
    [InlineData(null)]
    public void OnGet_should_replace_return_path_when_it_is_invalid(string? path)
    {
        _sut.ReturnPath = path;
        _sut.OnGet();
        _sut.ReturnPath.Should().Be("/");
    }

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("https://www.gov.uk")]
    [InlineData(null)]
    public void OnPost_should_replace_return_path_when_it_is_invalid(string? path)
    {
        _sut.ReturnPath = path;
        _sut.OnPost();
        _sut.ReturnPath.Should().Be("/");
    }

    //Check Consent is set correctly

    [Theory]
    [InlineData(true, null)]
    [InlineData(false, null)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void OnGet_should_not_change_consent_when_it_is_provided(bool consent, bool? accepted)
    {
        _mockHttpContext.SetupConsentCookie(accepted);
        _sut.Consent = consent;
        _sut.OnGet();
        _sut.Consent.Should().Be(consent);
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
        _mockHttpContext.SetupConsentCookie(accepted);
        _sut.Consent = consent;
        _sut.OnPost();
        _sut.Consent.Should().Be(consent);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnGet_should_set_consent_when_it_not_provided_and_the_cookie_has_been_set(bool accepted)
    {
        _mockHttpContext.SetupConsentCookie(accepted);
        _sut.OnGet();
        _sut.Consent.Should().Be(accepted);
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnGet_returns_local_redirect_when_consent_is_given(bool consent)
    {
        _sut.Consent = consent;
        var result = _sut.OnGet();
        result.Should().BeOfType<LocalRedirectResult>();
    }

    [Fact]
    public void OnGet_returns_Cookies_page_when_consent_is_not_given()
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

    // Consent cookie appended (value true or false depending on test)
    [Theory]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    public void OnGet_adds_consent_cookie_when_consent_is_given(bool consent, string value)
    {
        _sut.Consent = consent;
        _sut.OnGet();
        _mockHttpContext.VerifySecureCookieAdded(CookiesHelper.ConsentCookieName, value);
    }

    [Theory]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    public void OnPost_adds_consent_cookie_when_consent_is_given(bool consent, string value)
    {
        _sut.Consent = consent;
        _sut.OnPost();
        _mockHttpContext.VerifySecureCookieAdded(CookiesHelper.ConsentCookieName, value);
    }

    // (Cookie appended Delete called if needed/TempData is not null)
    [Theory]
    [InlineData("ai_session")]
    [InlineData("ai_user")]
    public void OnGet_removes_optional_cookie_when_consent_is_false(string cookieName)
    {
        _mockHttpContext.SetupOptionalCookies();
        _sut.Consent = false;
        _sut.OnGet();
        _mockHttpContext.VerifyCookieDeleted(cookieName);
    }

    [Theory]
    [InlineData("ai_session")]
    [InlineData("ai_user")]
    public void OnPost_removes_optional_cookie_when_consent_is_false(string cookieName)
    {
        _mockHttpContext.SetupOptionalCookies();
        _sut.Consent = false;
        _sut.OnPost();
        _mockHttpContext.VerifyCookieDeleted(cookieName);
    }

    [Fact]
    public void OnGet_does_not_remove_cookies_when_consent_is_true()
    {
        _mockHttpContext.SetupOptionalCookies();
        _mockHttpContext.SetupAcceptedCookie();
        _sut.Consent = true;
        _sut.OnGet();
        _mockHttpContext.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnPost_does_not_remove_cookies_when_consent_is_true()
    {
        _mockHttpContext.SetupOptionalCookies();
        _mockHttpContext.SetupAcceptedCookie();
        _sut.Consent = true;
        _sut.OnPost();
        _mockHttpContext.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnGet_does_not_remove_cookies_when_there_are_no_optional_cookies_to_remove()
    {
        _mockHttpContext.SetupRejectedCookie();
        _sut.Consent = false;
        _sut.OnGet();
        _mockHttpContext.VerifyNoCookiesDeleted();
    }

    [Fact]
    public void OnPost_does_not_remove_cookies_when_there_are_no_optional_cookies_to_remove()
    {
        _mockHttpContext.SetupRejectedCookie();
        _sut.Consent = false;
        _sut.OnPost();
        _mockHttpContext.VerifyNoCookiesDeleted();
    }

    //TempData
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnGet_sets_tempdata_cookie_changed_when_consent_is_given(bool consent)
    {
        _sut.Consent = consent;
        _sut.OnGet();
        Assert.NotNull(_tempData[CookiesHelper.CookieChangedTempDataName]);
        _tempData[CookiesHelper.CookieChangedTempDataName].As<bool>().Should().BeTrue();
    }

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
    public void OnGet_sets_tempdata_cookie_deleted_when_consent_is_false()
    {
        _sut.Consent = false;
        _sut.OnGet();
        _tempData[CookiesHelper.DeleteCookieTempDataName].Should().NotBeNull();
        _tempData[CookiesHelper.DeleteCookieTempDataName].As<bool>().Should().BeTrue();
    }

    [Fact]
    public void OnPost_sets_tempdata_cookie_deleted_when_consent_is_false()
    {
        _sut.Consent = false;
        _sut.OnPost();
        _tempData[CookiesHelper.DeleteCookieTempDataName].Should().NotBeNull();
        _tempData[CookiesHelper.DeleteCookieTempDataName].As<bool>().Should().BeTrue();
    }
}
