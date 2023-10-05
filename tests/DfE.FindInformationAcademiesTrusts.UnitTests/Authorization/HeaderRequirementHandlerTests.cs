using DfE.FindInformationAcademiesTrusts.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Authorization;

public class HeaderRequirementHandlerTests
{
    private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    private readonly Mock<IHttpContextAccessor> _mockHttpAccessor;
    private readonly DefaultHttpContext _httpContext;

    public HeaderRequirementHandlerTests()
    {
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        _mockHttpAccessor = new Mock<IHttpContextAccessor>();
        _httpContext = new DefaultHttpContext();

        _mockHttpAccessor.Setup(m => m.HttpContext).Returns(_httpContext);
    }

    [Theory]
    [InlineData("Development", true)]
    [InlineData("LocalDevelopment", true)]
    [InlineData("CI", true)]
    [InlineData("Test", true)]
    [InlineData("Production", false)]
    public void IsClientSecretHeaderValid_should_only_be_true_in_specified_environments(string environment,
        bool expected)
    {
        _mockWebHostEnvironment.SetupGet(m => m.EnvironmentName).Returns(environment);
        _httpContext.Request.Headers.Add(HeaderNames.Authorization, "Bearer 123");
        var configurationSettings = new TestOverrideOptions { PlaywrightTestSecret = "123" };

        var sut = new HeaderRequirementHandler(_mockWebHostEnvironment.Object, _mockHttpAccessor.Object,
            configurationSettings);

        var result = sut.IsClientSecretHeaderValid();

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("123", null)]
    [InlineData(null, "123")]
    [InlineData("123", "456")]
    [InlineData("", "")]
    public void ClientSecretHeaderValid_should_return_false_if_contents_are_wrong(string headerAuthKey,
        string serverAuthKey)
    {
        _mockWebHostEnvironment.SetupGet(m => m.EnvironmentName).Returns("Development");
        _httpContext.Request.Headers.Add(HeaderNames.Authorization, $"Bearer {headerAuthKey}");
        var configurationSettings = new TestOverrideOptions { PlaywrightTestSecret = serverAuthKey };

        var sut = new HeaderRequirementHandler(_mockWebHostEnvironment.Object, _mockHttpAccessor.Object,
            configurationSettings);

        var result = sut.IsClientSecretHeaderValid();

        result.Should().BeFalse();
    }
}
