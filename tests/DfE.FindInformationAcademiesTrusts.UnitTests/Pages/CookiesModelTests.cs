using DfE.FindInformationAcademiesTrusts.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class CookiesModelTests
{
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<IResponseCookies> _mockResponseCookies;
    private readonly Mock<ISession> _mockSession;
    private readonly CookiesModel _sut;
    private readonly Mock<IHttpContextAccessor> _mockHttpAccessor;


    public CookiesModelTests()
    {
        _mockHttpAccessor = new Mock<IHttpContextAccessor>();
        _httpContext = new DefaultHttpContext();
        _mockResponseCookies = new Mock<IResponseCookies>();
        _mockSession = new Mock<ISession>();
        _mockHttpAccessor.Setup(m => m.HttpContext).Returns(_httpContext);
        _mockHttpAccessor.Setup(m => m.HttpContext!.Session).Returns(_mockSession.Object);
        _mockHttpAccessor.Setup(m => m.HttpContext!.Response.Cookies).Returns(_mockResponseCookies.Object);
        _sut = new CookiesModel(_mockHttpAccessor.Object);
    }

    [Theory]
    [InlineData("page")]
    [InlineData("")]
    [InlineData(null)]
    public void Visit_cookies_page_from_footer(string page)
    {
        var result = _sut.OnGet(null, page);
        Assert.IsType<PageResult>(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Set_preferences_from_cookie_banner(bool? preferences)
    {
        var result = _sut.OnGet(preferences, "page");
        Assert.IsType<RedirectResult>(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Set_preferences_from_cookie_page(bool preferences)
    {
        var result = _sut.OnPost(preferences, "page");
        _sut.PreferencesSet.Should().Be(true);
        Assert.IsType<PageResult>(result);
    }
}
