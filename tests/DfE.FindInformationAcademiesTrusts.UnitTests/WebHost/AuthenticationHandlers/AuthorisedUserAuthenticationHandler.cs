using System.Security.Claims;
using System.Text.Encodings.Web;
using DfE.FindInformationAcademiesTrusts.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.WebHost.AuthenticationHandlers;

public class AuthorisedUserAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string AuthenticationScheme = "AuthorisedUser";

    /// <summary>
    /// The user authenticates successfully but doesn't have the "User.Role.Authorised" role
    /// </summary>
    /// <returns></returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new Claim[]
        {
            new("name", "Test user"),
            new("preferred_username", "Test User - email"),
            new(ClaimTypes.Role, UserRoles.AuthorisedFiatUser)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
