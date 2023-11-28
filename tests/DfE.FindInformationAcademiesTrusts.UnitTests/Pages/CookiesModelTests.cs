using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages;
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
    private readonly Mock<IResponseCookies> _mockResponseCookies;
    private readonly Mock<IRequestCookieCollection> _mockRequestCookies;
    private readonly CookiesModel _sut;
    private readonly TempDataDictionary _tempData;

    public CookiesModelTests()
    {
        Mock<IHttpContextAccessor> mockHttpAccessor = new();
        var httpContext = new DefaultHttpContext();
        _mockResponseCookies = new Mock<IResponseCookies>();
        _mockRequestCookies = new Mock<IRequestCookieCollection>();
        mockHttpAccessor.Setup(m => m.HttpContext).Returns(httpContext);
        mockHttpAccessor.Setup(m => m.HttpContext!.Response.Cookies).Returns(_mockResponseCookies.Object);
        mockHttpAccessor.Setup(m => m.HttpContext!.Request.Cookies).Returns(_mockRequestCookies.Object);
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        _sut = new CookiesModel(mockHttpAccessor.Object)
        {
            TempData = _tempData,
            Url = new UrlHelper(actionContext)
        };
    }

    private void SetupAcceptedCookie()
    {
        _mockRequestCookies.Setup(m => m.ContainsKey(CookiesExtensions.ConsentCookieName)).Returns(true);
        _mockRequestCookies.Setup(m => m[CookiesExtensions.ConsentCookieName]).Returns("True");
    }

    private void SetupRejectedCookie()
    {
        _mockRequestCookies.Setup(m => m.ContainsKey(CookiesExtensions.ConsentCookieName)).Returns(true);
        _mockRequestCookies.Setup(m => m[CookiesExtensions.ConsentCookieName]).Returns("False");
    }

    private void SetupOptionalCookies()
    {
        _mockRequestCookies.Setup(m => m.ContainsKey("ai_user")).Returns(true);
        _mockRequestCookies.Setup(m => m["ai_user"]).Returns("True");
        _mockRequestCookies.Setup(m => m.ContainsKey("ai_session")).Returns(true);
        _mockRequestCookies.Setup(m => m["ai_session"]).Returns("True");
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
        Assert.Equivalent(path, _sut.ReturnPath);
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
        Assert.Equivalent(path, _sut.ReturnPath);
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
        Assert.Equivalent("/", _sut.ReturnPath);
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
        Assert.Equivalent("/", _sut.ReturnPath);
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
        if (accepted == true)
        {
            SetupAcceptedCookie();
        }
        else if (accepted == false)
        {
            SetupRejectedCookie();
        }

        _sut.Consent = consent;
        _sut.OnGet();
        Assert.Equivalent(consent, _sut.Consent);
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
        if (accepted == true)
        {
            SetupAcceptedCookie();
        }
        else if (accepted == false)
        {
            SetupRejectedCookie();
        }

        _sut.Consent = consent;
        _sut.OnPost();
        Assert.Equal(consent, _sut.Consent);
    }

    //Check consent set when there is a previous consent value

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnGet_should_set_consent_when_it_not_provided_and_the_cookie_has_been_set(bool accepted)
    {
        if (accepted)
        {
            SetupAcceptedCookie();
        }
        else
        {
            SetupRejectedCookie();
        }

        _sut.OnGet();
        Assert.Equal(accepted, _sut.Consent);
    }

    [Fact]
    public void OnGet_should_not_set_consent_when_it_not_provided_and_the_cookie_has_not_been_set()
    {
        _sut.OnGet();
        Assert.Null(_sut.Consent);
    }

    [Fact]
    public void OnPost_should_not_set_consent_when_it_not_provided_and_the_cookie_has_not_been_set()
    {
        _sut.OnPost();
        Assert.Null(_sut.Consent);
    }

    //Check DisplayCookieChangedMessageOnCookiesPage is set correctly
    // True if post 
    // False if get 
    [Fact]
    public void OnGet_DisplayCookieChangedMessageOnCookiesPage_should_always_be_false()
    {
        _sut.OnGet();
        Assert.False(_sut.DisplayCookieChangedMessageOnCookiesPage);
    }

    [Fact]
    public void OnPost_DisplayCookieChangedMessageOnCookiesPage_should_always_be_true()
    {
        _sut.OnPost();
        Assert.True(_sut.DisplayCookieChangedMessageOnCookiesPage);
    }

    //On get returns correctly for redirect (banner/consent)
    //True if get and consent is given (with or without path)
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnGet_returns_local_redirect_when_consent_is_given(bool consent)
    {
        _sut.Consent = consent;
        var result = _sut.OnGet();
        Assert.IsType<LocalRedirectResult>(result);
    }

    //On get returns correctly for page load (no consent or no return path)
    //True if get and consent not given (with or without path)
    [Fact]
    public void OnGet_returns_Cookies_page_when_consent_is_not_given()
    {
        var result = _sut.OnGet();
        Assert.IsType<PageResult>(result);
    }

    //On post returns correctly
    //True if post, consent is given or not, path is given or not
    [Fact]
    public void OnPost_returns_Cookies_page()
    {
        var result = _sut.OnPost();
        Assert.IsType<PageResult>(result);
    }

    // Consent cookie appended (value true or false depending on test)
    [Theory]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    public void OnGet_adds_consent_cookie_when_consent_is_given(bool consent, string value)
    {
        _sut.Consent = consent;
        _sut.OnGet();
        _mockResponseCookies.Verify(
            m => m.Append(CookiesExtensions.ConsentCookieName, value, It.IsAny<CookieOptions>()), Times.Once);
    }

    [Theory]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    public void OnPost_adds_consent_cookie_when_consent_is_given(bool consent, string value)
    {
        _sut.Consent = consent;
        _sut.OnPost();
        _mockResponseCookies.Verify(
            m => m.Append(CookiesExtensions.ConsentCookieName, value, It.IsAny<CookieOptions>()), Times.Once);
    }

    // (Cookie appended Delete called if needed/TempData is not null)
    [Fact]
    public void OnGet_removes_cookies_when_consent_is_false()
    {
        SetupOptionalCookies();
        _sut.Consent = false;
        _sut.OnGet();
        _mockResponseCookies.Verify(
            m => m.Delete(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void OnPost_removes_cookies_when_consent_is_false()
    {
        SetupOptionalCookies();
        _sut.Consent = false;
        _sut.OnPost();
        _mockResponseCookies.Verify(
            m => m.Delete(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void OnGet_does_not_removes_cookies_when_consent_is_true()
    {
        SetupOptionalCookies();
        _sut.Consent = true;
        _sut.OnGet();
        _mockResponseCookies.Verify(
            m => m.Delete(It.IsAny<string>()), Times.Exactly(0));
    }

    [Fact]
    public void OnPost_does_not_removes_cookies_when_consent_is_true()
    {
        SetupOptionalCookies();
        _sut.Consent = true;
        _sut.OnPost();
        _mockResponseCookies.Verify(
            m => m.Delete(It.IsAny<string>()), Times.Exactly(0));
    }

    [Fact]
    public void OnGet_does_not_removes_cookies_when_there_are_no_cookies_to_remove()
    {
        _sut.Consent = false;
        _sut.OnGet();
        _mockResponseCookies.Verify(
            m => m.Delete(It.IsAny<string>()), Times.Exactly(0));
    }

    [Fact]
    public void OnPost_does_not_removes_cookies_when_there_are_no_cookies_to_remove()
    {
        _sut.Consent = false;
        _sut.OnPost();
        _mockResponseCookies.Verify(
            m => m.Delete(It.IsAny<string>()), Times.Exactly(0));
    }

    //TempData
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnGet_sets_tempdata_cookie_changed_when_consent_is_given(bool consent)
    {
        _sut.Consent = consent;
        _sut.OnGet();
        Assert.NotNull(_tempData[CookiesExtensions.CookieChangedTempDataName]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnPost_sets_tempdata_cookie_changed_when_consent_is_given(bool consent)
    {
        _sut.Consent = consent;
        _sut.OnPost();
        Assert.NotNull(_tempData[CookiesExtensions.CookieChangedTempDataName]);
    }

    [Fact]
    public void OnGet_does_not_set_tempdata_cookie_changed_when_consent_is_not_given()
    {
        _sut.OnGet();
        Assert.Null(_tempData[CookiesExtensions.CookieChangedTempDataName]);
    }

    [Fact]
    public void OnPost_does_not_set_tempdata_cookie_changed_when_consent_is_not_given()
    {
        _sut.OnPost();
        Assert.Null(_tempData[CookiesExtensions.CookieChangedTempDataName]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(null)]
    public void OnGet_does_not_set_tempdata_cookie_deleted_when_consent_is_true_not_given(bool? consent)
    {
        _sut.Consent = consent;
        _sut.OnGet();
        Assert.Null(_tempData[CookiesExtensions.DeleteCookieTempDataName]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(null)]
    public void OnPost_does_not_set_tempdata_cookie_deleted_when_consent_is_true_not_given(bool? consent)
    {
        _sut.Consent = consent;
        _sut.OnPost();
        Assert.Null(_tempData[CookiesExtensions.DeleteCookieTempDataName]);
    }

    [Fact]
    public void OnGet_sets_tempdata_cookie_deleted_when_consent_is_false()
    {
        _sut.Consent = false;
        _sut.OnGet();
        Assert.NotNull(_tempData[CookiesExtensions.DeleteCookieTempDataName]);
    }

    [Fact]
    public void OnPost_sets_tempdata_cookie_deleted_when_consent_is_false()
    {
        _sut.Consent = false;
        _sut.OnPost();
        Assert.NotNull(_tempData[CookiesExtensions.DeleteCookieTempDataName]);
    }
}
