using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DfE.FindInformationAcademiesTrusts.Authorization;

public class HeaderRequirementHandler : AuthorizationHandler<DenyAnonymousAuthorizationRequirement>,
    IAuthorizationRequirement
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string? _playwrightTestSecret;
    private readonly bool _isLiveEnvironment;

    public HeaderRequirementHandler(IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor, IOptions<TestOverrideOptions> testOverrideOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _playwrightTestSecret = testOverrideOptions.Value.PlaywrightTestSecret;
        _isLiveEnvironment = environment.IsLiveEnvironment();
    }

    public bool IsClientSecretHeaderValid()
    {
        if (string.IsNullOrWhiteSpace(_playwrightTestSecret) || _isLiveEnvironment)
            return false;

        var requestHeader = _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization];
        if (string.IsNullOrWhiteSpace(requestHeader))
            return false;

        var authHeader = requestHeader.Value.ToString().Replace("Bearer ", string.Empty);

        return authHeader == _playwrightTestSecret;
    }

    [ExcludeFromCodeCoverage] // This method is difficult to test, everything that can be tested has been extracted to IsClientSecretHeaderValid
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        DenyAnonymousAuthorizationRequirement requirement)
    {
        if (IsClientSecretHeaderValid())
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
