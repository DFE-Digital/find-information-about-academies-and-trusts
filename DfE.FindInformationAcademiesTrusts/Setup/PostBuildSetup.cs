using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Configuration;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

namespace DfE.FindInformationAcademiesTrusts.Setup;

[ExcludeFromCodeCoverage]
public static class PostBuildSetup
{
    public static void ConfigureApp(WebApplication app)
    {
        ProxyForwardedHeadersSetup.UseForwardedHeadersForProxy(app);

        // Set HTTP Security headers
        app.UseSecurityHeaders(SecurityHeaderPolicy.GetSecurityHeaderPolicies());

        if (!app.Environment.IsLocalDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always,
            HttpOnly = HttpOnlyPolicy.Always,
            MinimumSameSitePolicy = SameSiteMode.None
        });

        app.UseHttpsRedirection();
        app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

        app.UseStaticFiles();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.UseMiddleware<ResponseHeadersMiddleware>();
        app.MapHealthChecks("/health").AllowAnonymous();

        // Only setup Azure App Configuration if the Connection string is present 
        var appConfigString = app.Configuration["AppConfig"];
        if (!string.IsNullOrWhiteSpace(appConfigString))
        {
            app.UseAzureAppConfiguration();
        }
    }
}
