using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockHttpContext : Mock<HttpContext>
{
    public MockResponseCookies MockResponseCookies { get; } = new();
    public MockRequestCookies MockRequestCookies { get; } = new();
    private readonly Mock<HttpRequest> _mockRequest = new();
    private readonly Mock<IFeatureCollection> _mockFeatureCollection = new();
    private readonly ClaimsIdentity _claimsIdentity = new();

    public MockHttpContext()
    {
        Mock<HttpResponse> mockResponse = new();
        mockResponse.Setup(m => m.Cookies).Returns(MockResponseCookies.Object);

        ClaimsPrincipal user = new();
        user.AddIdentity(_claimsIdentity);

        _mockRequest.Setup(m => m.Cookies).Returns(MockRequestCookies.Object);
        _mockRequest.Setup(m => m.Query[It.IsAny<string>()]).Returns("");

        Setup(m => m.Request).Returns(_mockRequest.Object);
        Setup(m => m.Response).Returns(mockResponse.Object);
        Setup(m => m.Features).Returns(_mockFeatureCollection.Object);
        Setup(m => m.User).Returns(user);
    }

    public void AddUserClaim(string type, string value)
    {
        _claimsIdentity.AddClaim(new Claim(type, value));
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
}
