using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class ResponseHeadersMiddlewareTests
{
    private readonly RequestDelegate _mockRequestDelegate;
    private readonly ResponseHeadersMiddleware _sut;
    private readonly HttpContext _mockContext;

    public ResponseHeadersMiddlewareTests()
    {
        _mockRequestDelegate = Substitute.For<RequestDelegate>();
        _sut = new ResponseHeadersMiddleware(_mockRequestDelegate);
        _mockContext = Substitute.For<HttpContext>();

        var responseHeaders = new HeaderDictionary();
        var mockResponse = Substitute.For<HttpResponse>();
        mockResponse.Headers.Returns(responseHeaders);
        _mockContext.Response.Returns(mockResponse);
    }

    [Fact]
    public async Task Invoke_should_return_next_delegate()
    {
        await _sut.Invoke(_mockContext);
        await _mockRequestDelegate.Received().Invoke(_mockContext);
    }

    [Theory]
    [InlineData("X-Robots-Tag")]
    [InlineData("Some-Other-Header")]
    public async Task Invoke_should_not_override_existing_headers(string headerName)
    {
        _mockContext.Response.Headers[headerName] = "existing header";

        await _sut.Invoke(_mockContext);

        _mockContext.Response.Headers[headerName].Should().ContainSingle().Which.Should().Be("existing header");
    }

    [Fact]
    public async Task Invoke_should_set_XRobotTag_header_to_noindex_nofollow()
    {
        await _sut.Invoke(_mockContext);
        _mockContext.Response.Headers["X-Robots-Tag"].Should().ContainSingle().Which.Should()
            .Be("noindex, nofollow");
    }
}
