using System.Security.Claims;
using DfE.FindInformationAcademiesTrusts.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockHttpContext : Mock<HttpContext>
{
    private readonly Mock<IResponseCookies> _mockResponseCookies = new();
    private readonly Mock<IRequestCookieCollection> _mockRequestCookies = new();
    private readonly Mock<HttpRequest> _mockRequest = new();
    private readonly Mock<IFeatureCollection> _mockFeatureCollection = new();
    private readonly ClaimsIdentity _claimsIdentity = new();
    private readonly Dictionary<string, string> _requestCookies = new();

    public MockHttpContext()
    {
        Mock<HttpResponse> mockResponse = new();
        mockResponse.Setup(m => m.Cookies).Returns(_mockResponseCookies.Object);

        ClaimsPrincipal user = new();
        user.AddIdentity(_claimsIdentity);

        _mockRequest.Setup(m => m.Cookies).Returns(_mockRequestCookies.Object);
        _mockRequest.Setup(m => m.Query[It.IsAny<string>()]).Returns("");

        _mockRequestCookies.Setup(m => m.Keys).Returns(_requestCookies.Keys);
        _mockRequestCookies.Setup(m => m.ContainsKey(It.IsAny<string>()))
            .Returns((string key) => _requestCookies.ContainsKey(key));
        _mockRequestCookies.Setup(m => m[It.IsAny<string>()])
            .Returns((string key) => _requestCookies.TryGetValue(key, out var value) ? value : null);

        Setup(m => m.Request).Returns(_mockRequest.Object);
        Setup(m => m.Response).Returns(mockResponse.Object);
        Setup(m => m.Features).Returns(_mockFeatureCollection.Object);
        Setup(m => m.User).Returns(user);

        SetUserTo(UserAuthState.Authorised);
    }

    public void AddRequestCookie(string key, string value)
    {
        _requestCookies.Add(key, value);
    }

    public enum UserAuthState
    {
        Authorised,
        Unauthorised,
        Unauthenticated
    }

    public void SetUserTo(UserAuthState value)
    {
        //Reset user state
        var roleClaim = _claimsIdentity.Claims.SingleOrDefault(c => c.Type == _claimsIdentity.RoleClaimType);
        if (roleClaim is not null) _claimsIdentity.RemoveClaim(roleClaim);
        _requestCookies.Remove(FiatCookies.Login);

        // Set user to correct state
        switch (value)
        {
            case UserAuthState.Authorised:
                AddUserClaim(_claimsIdentity.RoleClaimType, "User.Role.Authorised");
                AddRequestCookie(FiatCookies.Login, "You are logged in");
                break;
            case UserAuthState.Unauthorised:
                AddRequestCookie(FiatCookies.Login, "You are logged in");
                break;
            case UserAuthState.Unauthenticated:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }

    public void AddUserClaim(string type, string value)
    {
        _claimsIdentity.AddClaim(new Claim(type, value));
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
        AddRequestCookie(FiatCookies.CookieConsent, "True");
    }

    public void SetupRejectedCookie()
    {
        AddRequestCookie(FiatCookies.CookieConsent, "False");
    }

    public void SetupOptionalCookies()
    {
        AddRequestCookie("ai_user", "True");
        AddRequestCookie("ai_session", "True");
        AddRequestCookie("_gid", "True");
        AddRequestCookie("_ga", "True");
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

    public void SetNotFoundUrl(string host, string path, string query)
    {
        _mockFeatureCollection.Setup(m => m.Get<IStatusCodeReExecuteFeature>())
            .Returns(Of<IStatusCodeReExecuteFeature>(m =>
                m.OriginalPath == path
                && m.OriginalQueryString == query));

        _mockRequest.Setup(m => m.Host).Returns(new HostString(host));
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
