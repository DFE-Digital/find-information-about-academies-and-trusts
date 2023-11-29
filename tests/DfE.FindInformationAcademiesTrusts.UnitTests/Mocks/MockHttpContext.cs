using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockHttpContext : Mock<HttpContext>
{
    private readonly Mock<IResponseCookies> _mockResponseCookies = new();
    private readonly Mock<IRequestCookieCollection> _mockRequestCookies = new();
    private readonly Mock<HttpRequest> _mockRequest = new();

    public MockHttpContext()
    {
        Mock<HttpResponse> mockResponse = new();

        Setup(m => m.Request).Returns(_mockRequest.Object);
        Setup(m => m.Response).Returns(mockResponse.Object);

        mockResponse.Setup(m => m.Cookies).Returns(_mockResponseCookies.Object);
        _mockRequest.Setup(m => m.Cookies).Returns(_mockRequestCookies.Object);
        _mockRequestCookies.Setup(m => m[It.IsAny<string>()]).Returns("False");
        _mockRequestCookies.Setup(m => m.ContainsKey(".FindInformationAcademiesTrusts.Login")).Returns(true);
        _mockRequestCookies.Setup(m => m[".FindInformationAcademiesTrusts.Login"]).Returns("You are logged in");
        _mockRequest.Setup(m => m.Query[It.IsAny<string>()]).Returns("");
    }

    public void SetupConsentCookie(bool? accepted)
    {
        if (accepted is true)
        {
            SetupAcceptedCookie();
        }
        else if (accepted is false)
        {
            SetupRejectedCookie();
        }
    }

    public void SetupAcceptedCookie()
    {
        _mockRequestCookies.Setup(m => m.ContainsKey(CookiesHelper.ConsentCookieName)).Returns(true);
        _mockRequestCookies.Setup(m => m[CookiesHelper.ConsentCookieName]).Returns("True");
    }

    public void SetupRejectedCookie()
    {
        _mockRequestCookies.Setup(m => m.ContainsKey(CookiesHelper.ConsentCookieName)).Returns(true);
        _mockRequestCookies.Setup(m => m[CookiesHelper.ConsentCookieName]).Returns("False");
    }

    public void SetupOptionalCookies()
    {
        _mockRequestCookies.Setup(m => m.ContainsKey("ai_user")).Returns(true);
        _mockRequestCookies.Setup(m => m["ai_user"]).Returns("True");
        _mockRequestCookies.Setup(m => m.ContainsKey("ai_session")).Returns(true);
        _mockRequestCookies.Setup(m => m["ai_session"]).Returns("True");
    }

    public void SetQueryReturnPath(string path)
    {
        _mockRequest.Setup(m => m.Query[CookiesHelper.ReturnPathQuery]).Returns(path);
    }

    public void SetPath(string path)
    {
        if (path[0] is not '/')
        {
            path = "/" + path;
        }

        _mockRequest.Setup(m => m.Path).Returns(path);
    }

    public void SetQueryString(string queryString)
    {
        if (queryString[0] is not '?')
        {
            queryString = "?" + queryString;
        }

        _mockRequest.Setup(m => m.QueryString).Returns(new QueryString(queryString));
    }

    public void VerifySecureCookieAdded(string key, string value)
    {
        _mockResponseCookies.Verify(
            m => m.Append(key, value,
                It.Is<CookieOptions>(c => c.Secure == true && c.HttpOnly == true)), Times.Once);
    }

    public void VerifyCookieDeleted(string key)
    {
        _mockResponseCookies.Verify(
            m => m.Delete(key), Times.Once);
    }

    public void VerifyNoCookiesDeleted()
    {
        _mockResponseCookies.Verify(
            m => m.Delete(It.IsAny<string>()), Times.Exactly(0));
    }
}
