using System.Security.Claims;
using DfE.FindInformationAcademiesTrusts.Authorization;
using DfE.FindInformationAcademiesTrusts.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Authorization;

public class AutomationAuthorizationHandlerTests
{
    private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<IOptions<TestOverrideOptions>> _mockTestOverrideOptions;
    private readonly AutomationAuthorizationHandler _sut;
    private readonly Mock<IHttpContextAccessor> _mockHttpAccessor;

    public AutomationAuthorizationHandlerTests()
    {
        _mockHttpAccessor = new Mock<IHttpContextAccessor>();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        _mockTestOverrideOptions = new Mock<IOptions<TestOverrideOptions>>();
        _httpContext = new DefaultHttpContext();

        _mockHttpAccessor.Setup(m => m.HttpContext).Returns(_httpContext);
        _mockTestOverrideOptions.Setup(m => m.Value)
            .Returns(new TestOverrideOptions { CypressTestSecret = "123" });
        _mockWebHostEnvironment.SetupGet(m => m.EnvironmentName).Returns("Development");

        _sut = new AutomationAuthorizationHandler(_mockWebHostEnvironment.Object, _mockHttpAccessor.Object,
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
        _httpContext.Request.Headers.Append(HeaderNames.Authorization, "Bearer 123");

        //Create sut here because constructor decides whether or not an environment is live
        var sut = new AutomationAuthorizationHandler(_mockWebHostEnvironment.Object, _mockHttpAccessor.Object,
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
        _httpContext.Request.Headers.Append(HeaderNames.Authorization, $"Bearer {headerAuthKey}");

        var result = _sut.IsClientSecretHeaderValid();

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("Bearer ", null)]
    [InlineData("", "")]
    [InlineData("Bearer ", "")]
    public void ClientSecretHeaderValid_should_return_false_if_serverAuthKey_not_set(string headerAuthKey,
        string? serverAuthKey)
    {
        _httpContext.Request.Headers.Append(HeaderNames.Authorization, $"Bearer {headerAuthKey}");
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

    [Fact]
    public void SetupAutomationUser_sets_claims_from_user_context_header_when_no_roles()
    {
        _httpContext.Request.Headers.Append("x-user-context",
            """{"name": "Unauthorised Automation User - name", "email": "Unauthorised Automation User - email", "roles": []}""");

        _sut.SetupAutomationUser();
        var actual = _httpContext.User;

        // Exclude subject due to cyclical references
        actual.Claims.Should().BeEquivalentTo(new Claim[]
        {
            new("name", "Unauthorised Automation User - name"),
            new("preferred_username", "Unauthorised Automation User - email")
        }, options => options.Excluding(claim => claim.Subject));
    }

    [Fact]
    public void SetupAutomationUser_sets_claims_from_user_context_header_when_authorised_role()
    {
        _httpContext.Request.Headers.Append("x-user-context",
            """{"name": "Automation User - name", "email": "Automation User - email", "roles": ["User.Role.Authorised"]}""");

        _sut.SetupAutomationUser();
        var actual = _httpContext.User;

        // Exclude subject due to cyclical references
        actual.Claims.Should().BeEquivalentTo(new Claim[]
        {
            new("name", "Automation User - name"),
            new("preferred_username", "Automation User - email"),
            new("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User.Role.Authorised")
        }, options => options.Excluding(claim => claim.Subject));
    }
}
