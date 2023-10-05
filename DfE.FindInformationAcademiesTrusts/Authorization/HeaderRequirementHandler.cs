using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DfE.FindInformationAcademiesTrusts.Authorization;

//Handler is registered from the method RequireAuthenticatedUser()
public class HeaderRequirementHandler : AuthorizationHandler<DenyAnonymousAuthorizationRequirement>,
   IAuthorizationRequirement
{
   //private readonly IConfiguration _configuration;
   private readonly IWebHostEnvironment _environment;
   private readonly IHttpContextAccessor _httpContextAccessor;
   private readonly TestOverrideOptions _testOverrideOptions;

   public HeaderRequirementHandler(IWebHostEnvironment environment,
                                   IHttpContextAccessor httpContextAccessor,TestOverrideOptions testOverrideOptions)
   {
      _environment = environment;
      _httpContextAccessor = httpContextAccessor;
      // _configuration = configuration;
      _testOverrideOptions = testOverrideOptions;
   }
   public bool ClientSecretHeaderValid()
   {
     
      if (!_environment.IsLocalDevelopment() && !_environment.IsDevelopment() && !_environment.IsContinuousIntegration() && !_environment.IsTest())
      {
         return false;
      }

      string? authHeader = _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
      
      string? secret = _testOverrideOptions.PlaywrightTestSecret;

      if (string.IsNullOrWhiteSpace(authHeader) || string.IsNullOrWhiteSpace(secret))
      {
         return false;
      }

      return authHeader == secret;
   }

   protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                  DenyAnonymousAuthorizationRequirement requirement)
   {
      if (ClientSecretHeaderValid())
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
   }
}
