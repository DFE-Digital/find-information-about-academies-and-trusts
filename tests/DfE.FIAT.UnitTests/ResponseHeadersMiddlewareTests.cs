using DfE.FIAT.Web;
using Microsoft.AspNetCore.Http;

namespace DfE.FIAT.UnitTests;

public class ResponseHeadersMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockRequestDelegate;
    private readonly ResponseHeadersMiddleware _sut;
    private readonly Mock<HttpContext> _mockContext;

    public ResponseHeadersMiddlewareTests()
    {
        _mockRequestDelegate = new Mock<RequestDelegate>();
        _sut = new ResponseHeadersMiddleware(_mockRequestDelegate.Object);
        _mockContext = new Mock<HttpContext>();

        var responseHeaders = new HeaderDictionary();
        var mockResponse = new Mock<HttpResponse>();
        mockResponse.SetupGet(r => r.Headers).Returns(responseHeaders);
        _mockContext.SetupGet(c => c.Response).Returns(mockResponse.Object);
    }

    [Fact]
    public async Task Invoke_should_return_next_delegate()
    {
        await _sut.Invoke(_mockContext.Object);
        _mockRequestDelegate.Verify(r => r.Invoke(_mockContext.Object));
    }

    [Theory]
    [InlineData("X-Robots-Tag")]
    [InlineData("Some-Other-Header")]
    public async Task Invoke_should_not_override_existing_headers(string headerName)
    {
        _mockContext.Object.Response.Headers[headerName] = "existing header";

        await _sut.Invoke(_mockContext.Object);

        _mockContext.Object.Response.Headers[headerName].Should().ContainSingle().Which.Should().Be("existing header");
    }

    [Fact]
    public async Task Invoke_should_set_XRobotTag_header_to_noindex_nofollow()
    {
        await _sut.Invoke(_mockContext.Object);
        _mockContext.Object.Response.Headers["X-Robots-Tag"].Should().ContainSingle().Which.Should()
            .Be("noindex, nofollow");
    }
}
