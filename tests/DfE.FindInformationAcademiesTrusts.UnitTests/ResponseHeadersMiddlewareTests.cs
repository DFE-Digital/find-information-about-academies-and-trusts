using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

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

    [Fact]
    public async Task Invoke_should_set_XFrameOptions_to_deny()
    {
        await _sut.Invoke(_mockContext.Object);
        _mockContext.Object.Response.Headers.XFrameOptions.Should().ContainSingle().Which.Should().Be("deny");
    }

    [Fact]
    public async Task Invoke_should_set_XContentTypeOptions_to_nosniff()
    {
        await _sut.Invoke(_mockContext.Object);
        _mockContext.Object.Response.Headers.XContentTypeOptions.Should().ContainSingle().Which.Should().Be("nosniff");
    }

    [Fact]
    public async Task Invoke_should_set_ContentSecurityPolicy_to_be_secure()
    {
        await _sut.Invoke(_mockContext.Object);
        _mockContext.Object.Response.Headers.ContentSecurityPolicy.Should().ContainSingle().Which.Should().Be(
            "default-src 'self'; form-action 'self'; object-src 'none'; frame-ancestors 'none'");
    }

    [Theory]
    [InlineData("Referrer-Policy", "no-referrer")]
    [InlineData("Permissions-Policy",
        "accelerometer=(),ambient-light-sensor=(),autoplay=(),battery=(),camera=(),display-capture=(),document-domain=(),encrypted-media=(),fullscreen=(),gamepad=(),geolocation=(),gyroscope=(),layout-animations=(),legacy-image-formats=(),magnetometer=(),microphone=(),midi=(),oversized-images=(),payment=(),picture-in-picture=(),publickey-credentials-get=(),speaker-selection=(),sync-xhr=(),unoptimized-images=(),unsized-media=(),usb=(),screen-wake-lock=(),web-share=(),xr-spatial-tracking=()")]
    [InlineData("X-Permitted-Cross-Domain-Policies", "none")]
    [InlineData("X-Robots-Tag", "noindex, nofollow")]
    [InlineData("Cross-Origin-Embedder-Policy", "require-corp")]
    [InlineData("Cross-Origin-Opener-Policy", "same-origin")]
    [InlineData("Cross-Origin-Resource-Policy", "same-origin")]
    public async Task Invoke_should_set_other_security_headers_to_correct_values(string headername,
        string expectedValue)
    {
        await _sut.Invoke(_mockContext.Object);
        _mockContext.Object.Response.Headers[headername].Should().ContainSingle().Which.Should().Be(expectedValue);
    }
}
