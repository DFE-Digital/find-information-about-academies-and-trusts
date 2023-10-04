using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DfE.FindInformationAcademiesTrusts.Authorization;

public class ClaimsRequirementHandler : AuthorizationHandler<ClaimsAuthorizationRequirement>, IAuthorizationRequirement
{
   private readonly IConfiguration _configuration;
   private readonly IHostEnvironment _environment;
   private readonly IHttpContextAccessor _httpContextAccessor;

   public ClaimsRequirementHandler(IHostEnvironment environment,
                                   IHttpContextAccessor httpContextAccessor,
                                   IConfiguration configuration)
   {
      _environment = environment;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
   }

   protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimsAuthorizationRequirement requirement)
   {
      if (HeaderRequirementHandler.ClientSecretHeaderValid(_environment, _httpContextAccessor, _configuration))
      {
         context.Succeed(requirement);
      }

      return Task.CompletedTask;
   }
}
