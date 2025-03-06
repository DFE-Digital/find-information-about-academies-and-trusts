using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockHttpContext
{
    public MockResponseCookies MockResponseCookies { get; } = new();
    public MockRequestCookies MockRequestCookies { get; } = new();
    public HttpContext Object { get; } = Substitute.For<HttpContext>();
    
    private readonly HttpRequest _mockRequest = Substitute.For<HttpRequest>();
    private readonly IFeatureCollection _mockFeatureCollection = Substitute.For<IFeatureCollection>();
    private readonly ClaimsIdentity _claimsIdentity = new();

    public MockHttpContext()
    {
        HttpResponse mockResponse = Substitute.For<HttpResponse>();
        mockResponse.Cookies.Returns(MockResponseCookies.Cookies);
        
        ClaimsPrincipal user = new();
        user.AddIdentity(_claimsIdentity);
        
        _mockRequest.Cookies.Returns(MockRequestCookies);
        _mockRequest.Query[Arg.Any<string>()].Returns((StringValues)"");
        
        Object.Request.Returns(_mockRequest);
        Object.Response.Returns(mockResponse);
        Object.Features.Returns(_mockFeatureCollection);
        Object.User.Returns(user);
    }

    public void AddUserClaim(string type, string value)
    {
        _claimsIdentity.AddClaim(new Claim(type, value));
    }

    public void SetQueryReturnPath(string path)
    {
        _mockRequest.Query[CookiesHelper.ReturnPathQuery].Returns((StringValues)path);
    }

    public void SetPath(string path)
    {
        if (path[0] is not '/')
        {
            path = "/" + path;
        }
        
        _mockRequest.Path.Returns((PathString)path);
    }

    public void SetQueryString(string queryString)
    {
        if (queryString[0] is not '?')
        {
            queryString = "?" + queryString;
        }

        _mockRequest.QueryString.Returns(new QueryString(queryString));
    }

    public void SetNotFoundUrl(string host, string path, string query)
    {
        var statusCodeReExecuteFeature = Substitute.For<IStatusCodeReExecuteFeature>();
        statusCodeReExecuteFeature.OriginalPath.Returns(path);
        statusCodeReExecuteFeature.OriginalQueryString.Returns(query);
        
        _mockFeatureCollection.Get<IStatusCodeReExecuteFeature>().Returns(statusCodeReExecuteFeature);
        
        _mockRequest.Host.Returns(new HostString(host));
    }
}
