using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DfE.FindInformationAcademiesTrusts.Authorization;

public class AutomationAuthorizationHandler(
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor,
    IOptions<TestOverrideOptions> testOverrideOptions)
    : AuthorizationHandler<IAuthorizationRequirement>
{
    private sealed record AutomationUserContext(string Name, string Email, string[] Roles);

    private static readonly JsonSerializerOptions WebJsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly string? _cypressTestSecret = testOverrideOptions.Value.CypressTestSecret;
    private readonly bool _isLiveEnvironment = environment.IsLiveEnvironment();

    public bool IsClientSecretHeaderValid()
    {
        if (string.IsNullOrWhiteSpace(_cypressTestSecret) || _isLiveEnvironment)
            return false;

        var requestHeader = httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization];
        if (string.IsNullOrWhiteSpace(requestHeader))
            return false;

        var authHeader = requestHeader.Value.ToString().Replace("Bearer ", string.Empty);

        return authHeader == _cypressTestSecret;
    }

    public void SetupAutomationUser()
    {
        var httpContext = httpContextAccessor.HttpContext!;

        var userContextJson = httpContext.Request.Headers["x-user-context"].ToString();
        var automationUserContext =
            JsonSerializer.Deserialize<AutomationUserContext>(userContextJson, WebJsonSerializerOptions);

        if (automationUserContext == null)
            throw new InvalidOperationException("Could not deserialize automation user context");

        var identity = new ClaimsIdentity(new List<Claim>
        {
            new("name", automationUserContext.Name),
            new("preferred_username", automationUserContext.Email)
        });
        foreach (var role in automationUserContext.Roles)
            identity.AddClaim(new Claim(ClaimTypes.Role, role));

        var user = new ClaimsPrincipal(identity);

        httpContext.User = user;
    }

    [ExcludeFromCodeCoverage] // This method is difficult to test, everything that can be tested has been extracted to other public methods
    public override Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (IsClientSecretHeaderValid())
        {
            SetupAutomationUser();
            foreach (var requirement in context.Requirements)
                context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    [ExcludeFromCodeCoverage] // This method is difficult to test because it is protected and not invoked
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IAuthorizationRequirement requirement)
    {
        throw new NotImplementedException();
    }
}
