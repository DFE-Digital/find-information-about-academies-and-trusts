using DfE.FindInformationAcademiesTrusts.Authorization;
using DfE.FindInformationAcademiesTrusts.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Authorization;

public class HeaderRequirementHandlerTests
{
    private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<IOptions<TestOverrideOptions>> _mockTestOverrideOptions;
    private readonly HeaderRequirementHandler _sut;
    private readonly Mock<IHttpContextAccessor> _mockHttpAccessor;

    public HeaderRequirementHandlerTests()
    {
        _mockHttpAccessor = new Mock<IHttpContextAccessor>();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        _mockTestOverrideOptions = new Mock<IOptions<TestOverrideOptions>>();
        _httpContext = new DefaultHttpContext();

        _mockHttpAccessor.Setup(m => m.HttpContext).Returns(_httpContext);
        _mockTestOverrideOptions.Setup(m => m.Value)
            .Returns(new TestOverrideOptions { CypressTestSecret = "123" });
        _mockWebHostEnvironment.SetupGet(m => m.EnvironmentName).Returns("Development");

        _sut = new HeaderRequirementHandler(_mockWebHostEnvironment.Object, _mockHttpAccessor.Object,
            _mockTestOverrideOptions.Object);
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

        //Create sut here because constructor decides whether or not an environment is live
        var sut = new HeaderRequirementHandler(_mockWebHostEnvironment.Object, _mockHttpAccessor.Object,
            _mockTestOverrideOptions.Object);

        var result = sut.IsClientSecretHeaderValid();

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Bearer ")]
    [InlineData("Bearer 456")]
    public void ClientSecretHeaderValid_should_return_false_if_contents_are_wrong(string headerAuthKey)
    {
        _httpContext.Request.Headers.Add(HeaderNames.Authorization, $"Bearer {headerAuthKey}");

        var result = _sut.IsClientSecretHeaderValid();

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("Bearer ", null)]
    [InlineData("", "")]
    [InlineData("Bearer ", "")]
    public void ClientSecretHeaderValid_should_return_false_if_serverAuthKey_not_set(string headerAuthKey,
        string serverAuthKey)
    {
        _httpContext.Request.Headers.Add(HeaderNames.Authorization, $"Bearer {headerAuthKey}");
        _mockTestOverrideOptions.Setup(m => m.Value)
            .Returns(new TestOverrideOptions { CypressTestSecret = serverAuthKey });

        var result = _sut.IsClientSecretHeaderValid();

        result.Should().BeFalse();
    }

    [Fact]
    public void ClientSecretHeaderValid_should_return_false_if_no_auth_header_provided()
    {
        var result = _sut.IsClientSecretHeaderValid();

        result.Should().BeFalse();
    }
}
