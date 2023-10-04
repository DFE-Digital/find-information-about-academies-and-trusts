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
   private readonly IConfiguration _configuration;
   private readonly IHostEnvironment _environment;
   private readonly IHttpContextAccessor _httpContextAccessor;

   public HeaderRequirementHandler(IHostEnvironment environment,
                                   IHttpContextAccessor httpContextAccessor,
                                   IConfiguration configuration)
   {
      _environment = environment;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
   }
   public static bool ClientSecretHeaderValid(IHostEnvironment hostEnvironment,
                                              IHttpContextAccessor httpContextAccessor,
                                              IConfiguration configuration)
   {
     
      if (!hostEnvironment.IsStaging() && !hostEnvironment.IsDevelopment())
      {
         return false;
      }

      string? authHeader = httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
      
      string? secret = configuration.GetValue<string>("PlaywrightTestSecret");

      if (string.IsNullOrWhiteSpace(authHeader) || string.IsNullOrWhiteSpace(secret))
      {
         return false;
      }

      return authHeader == secret;
   }

   protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                  DenyAnonymousAuthorizationRequirement requirement)
   {
      if (ClientSecretHeaderValid(_environment, _httpContextAccessor, _configuration))
      {
        context.Succeed(requirement);
   
        string? headerRole = _httpContextAccessor.HttpContext?.Request.Headers["AuthorizationRole"].ToString();
         if (!string.IsNullOrWhiteSpace(headerRole))
         {
            string[] claims = headerRole.Split(',');
            foreach (string claim in claims)
            {
               context.User.Identities.FirstOrDefault()?.AddClaim(new Claim(ClaimTypes.Role, claim));
            }
         }
      }

      return Task.CompletedTask;
   }
}
