using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using DfE.FindInformationAcademiesTrusts.Configuration;
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
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new("name", "Automation User - name"),
            new("preferred_username", "Automation User - email"),
            new(ClaimTypes.Role, UserRoles.AuthorisedFiatUser)
        });
        var user = new ClaimsPrincipal(identity);

        httpContextAccessor.HttpContext!.User = user;
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
