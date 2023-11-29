using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class CookiesHelperTests
{
    private readonly Mock<HttpContext> _mockContext;
    private readonly Mock<ITempDataDictionary> _mockTempData;

    public CookiesHelperTests()
    {
        _mockContext = new Mock<HttpContext>();
        _mockTempData = new Mock<ITempDataDictionary>();
        _mockContext.Setup(m => m.Request.Cookies[CookiesHelper.ConsentCookieName]).Returns("False");
        _mockContext.Setup(m => m.Request.Query[It.IsAny<string>()]).Returns("");
    }

    private void SetTempDataCookieDeleted()
    {
        _mockTempData.Setup(m => m[CookiesHelper.DeleteCookieTempDataName]).Returns(true);
    }

    private void SetupAcceptedCookie()
    {
        _mockContext.Setup(m => m.Request.Cookies.ContainsKey(CookiesHelper.ConsentCookieName)).Returns(true);
        _mockContext.Setup(m => m.Request.Cookies[CookiesHelper.ConsentCookieName]).Returns("True");
    }

    private void SetupRejectedCookie()
    {
        _mockContext.Setup(m => m.Request.Cookies.ContainsKey(CookiesHelper.ConsentCookieName)).Returns(true);
        _mockContext.Setup(m => m.Request.Cookies[CookiesHelper.ConsentCookieName]).Returns("False");
    }

    private void SetQueryReturnPath(string path)
    {
        _mockContext.Setup(m => m.Request.Query[CookiesHelper.ReturnPathQuery]).Returns(path);
    }

    private void SetPath(string path)
    {
        if (path[0] is not '/')
        {
            path = "/" + path;
        }

        _mockContext.Setup(m => m.Request.Path).Returns(path);
    }

    private void SetQueryString(string queryString)
    {
        if (queryString[0] is not '?')
        {
            queryString = "?" + queryString;
        }

        _mockContext.Setup(m => m.Request.QueryString).Returns(new QueryString(queryString));
    }

    [Fact]
    public void OptionalCookiesAreAccepted_is_true_when_Accepted_cookie_exists()
    {
        SetupAcceptedCookie();
        var result = CookiesHelper.OptionalCookiesAreAccepted(_mockContext.Object, _mockTempData.Object);
        Assert.True(result);
    }

    [Fact]
    public void OptionalCookiesAreAccepted_is_false_when_Rejected_cookie_exists()
    {
        SetupRejectedCookie();
        var result = CookiesHelper.OptionalCookiesAreAccepted(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void OptionalCookiesAreAccepted_is_false_when_DeleteCookieTempData_exists(bool? cookieAccepted)
    {
        if (cookieAccepted == true)
        {
            SetupAcceptedCookie();
        }

        if (cookieAccepted == false)
        {
            SetupRejectedCookie();
        }

        SetTempDataCookieDeleted();
        var result = CookiesHelper.OptionalCookiesAreAccepted(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }

    [Fact]
    public void OptionalCookiesAreAccepted_is_false_when_neither_Cookie_or_TempData_exists()
    {
        var result = CookiesHelper.OptionalCookiesAreAccepted(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }

    [Fact]
    public void If_ReturnPath_Exists_in_query_then_ReturnPath_function_always_returns_it()
    {
        SetQueryReturnPath("Expected");
        SetPath("/Path");
        SetQueryString("?QueryString");
        var result = CookiesHelper.ReturnPath(_mockContext.Object);
        Assert.Equal("Expected", result);
    }

    [Fact]
    public void
        If_ReturnPath_does_not_exist_in_query_then_ReturnPath_function_always_returns_the_original_path_and_query()
    {
        SetPath("/Path");
        SetQueryString("?QueryString");
        var result = CookiesHelper.ReturnPath(_mockContext.Object);
        Assert.Equal("/Path?QueryString", result);
    }

    [Fact]
    public void ShowCookieBanner_is_true_when_consent_cookie_does_not_exist_and_temp_data_does_not_exist()
    {
        var result = CookiesHelper.ShowCookieBanner(_mockContext.Object, _mockTempData.Object);
        Assert.True(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowCookieBanner_is_false_when_consent_cookie_exists_and_temp_data_does_not_exist(bool cookieAccepted)
    {
        if (cookieAccepted)
        {
            SetupAcceptedCookie();
        }

        if (cookieAccepted == false)
        {
            SetupRejectedCookie();
        }

        var result = CookiesHelper.ShowCookieBanner(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }

    [Fact]
    public void ShowCookieBanner_is_false_when_consent_cookie_does_not_exist_and_temp_data_exists()
    {
        SetTempDataCookieDeleted();
        var result = CookiesHelper.ShowCookieBanner(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowCookieBanner_is_false_when_consent_cookie_exists_and_temp_data_exists(bool cookieAccepted)
    {
        if (cookieAccepted)
        {
            SetupAcceptedCookie();
        }

        if (cookieAccepted == false)
        {
            SetupRejectedCookie();
        }

        SetTempDataCookieDeleted();
        var result = CookiesHelper.ShowCookieBanner(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }
}
