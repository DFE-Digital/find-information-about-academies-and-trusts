using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class ResponseHeadersMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockRequestDelegate;
    private readonly ResponseHeadersMiddleware _sut;
    private readonly Mock<HttpContext> _mockContext;
    private readonly HeaderDictionary _responseHeaders;

    public ResponseHeadersMiddlewareTests()
    {
        _mockRequestDelegate = new Mock<RequestDelegate>();
        _sut = new ResponseHeadersMiddleware(_mockRequestDelegate.Object);
        _mockContext = new Mock<HttpContext>();
        _responseHeaders = new HeaderDictionary();

        var mockResponse = new Mock<HttpResponse>();
        mockResponse.SetupGet(r => r.Headers).Returns(_responseHeaders);
        _mockContext.SetupGet(c => c.Response).Returns(mockResponse.Object);
    }

    [Fact]
    public async Task Invoke_should_set_xframe_options_to_deny()
    {
        await _sut.Invoke(_mockContext.Object);
        _mockContext.Object.Response.Headers.XFrameOptions.Should().ContainSingle().Which.Should().Be("deny");
    }
}
