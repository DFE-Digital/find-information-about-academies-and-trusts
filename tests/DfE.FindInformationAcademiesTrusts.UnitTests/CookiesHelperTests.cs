using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class CookiesHelperTests
{
    private readonly MockHttpContext _mockContext;
    private readonly Mock<ITempDataDictionary> _mockTempData;

    public CookiesHelperTests()
    {
        _mockContext = new MockHttpContext();
        _mockTempData = new Mock<ITempDataDictionary>();
    }

    private void SetTempDataCookieDeleted()
    {
        _mockTempData.Setup(m => m[CookiesHelper.DeleteCookieTempDataName]).Returns(true);
    }

    [Fact]
    public void OptionalCookiesAreAccepted_is_true_when_Accepted_cookie_exists()
    {
        _mockContext.SetupAcceptedCookie();
        var result = CookiesHelper.OptionalCookiesAreAccepted(_mockContext.Object, _mockTempData.Object);
        Assert.True(result);
    }

    [Fact]
    public void OptionalCookiesAreAccepted_is_false_when_Rejected_cookie_exists()
    {
        _mockContext.SetupRejectedCookie();
        var result = CookiesHelper.OptionalCookiesAreAccepted(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void OptionalCookiesAreAccepted_is_false_when_DeleteCookieTempData_exists(bool? cookieAccepted)
    {
        _mockContext.SetupConsentCookie(cookieAccepted);

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
        _mockContext.SetQueryReturnPath("Expected");
        _mockContext.SetPath("/Path");
        _mockContext.SetQueryString("?QueryString");
        var result = CookiesHelper.ReturnPath(_mockContext.Object);
        Assert.Equal("Expected", result);
    }

    [Fact]
    public void
        If_ReturnPath_does_not_exist_in_query_then_ReturnPath_function_always_returns_the_original_path_and_query()
    {
        _mockContext.SetPath("/Path");
        _mockContext.SetQueryString("?QueryString");
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
        _mockContext.SetupConsentCookie(cookieAccepted);

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
        _mockContext.SetupConsentCookie(cookieAccepted);

        SetTempDataCookieDeleted();
        var result = CookiesHelper.ShowCookieBanner(_mockContext.Object, _mockTempData.Object);
        Assert.False(result);
    }
}
