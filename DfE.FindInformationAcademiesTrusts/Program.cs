using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DfE.FindInformationAcademiesTrusts.Authorization;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Serilog;

namespace DfE.FindInformationAcademiesTrusts;

[ExcludeFromCodeCoverage]
internal static class Program
{
    public static void Main(string[] args)
    {
        //Create logging mechanism before anything else to catch bootstrap errors
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            //Reconfigure logging before proceeding so any bootstrap exceptions can be written to App Insights 
            ReconfigureLogging(builder);

            AddEnvironmentVariablesTo(builder);

            builder.Services.AddRazorPages();
            builder.Services.AddHealthChecks();
            builder.Services.AddApplicationInsightsTelemetry();
            AddAuthenticationServices(builder);

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            AddDependenciesTo(builder);

            var app = builder.Build();
            ConfigureHttpRequestPipeline(app);
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureHttpRequestPipeline(WebApplication app)
    {
        if (!app.Environment.IsLocalDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseSecurityHeaders(GetSecurityHeaderPolicies());

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always, HttpOnly = HttpOnlyPolicy.Always,
            MinimumSameSitePolicy = SameSiteMode.None
        });

        app.UseHttpsRedirection();
        app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
        //For Azure AD redirect uri to remain https
        var forwardOptions = new ForwardedHeadersOptions
            { ForwardedHeaders = ForwardedHeaders.All, RequireHeaderSymmetry = false };
        forwardOptions.KnownNetworks.Clear();
        forwardOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardOptions);

        app.UseStaticFiles();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.UseMiddleware<ResponseHeadersMiddleware>();
        app.MapHealthChecks("/health").AllowAnonymous();
    }

    private static HeaderPolicyCollection GetSecurityHeaderPolicies()
    {
        return new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddXssProtectionBlock()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader()
            .AddContentSecurityPolicy(cspBuilder =>
            {
                cspBuilder.AddDefaultSrc().Self();
                cspBuilder.AddScriptSrc()
                    .Self()
                    .UnsafeInline()
                    .WithNonce()
                    .From("https://js.monitor.azure.com/scripts/b/ai.2.min.js");
                cspBuilder.AddConnectSrc()
                    .Self()
                    .From("https://*.in.applicationinsights.azure.com//v2/track");
                cspBuilder.AddObjectSrc().None();
                cspBuilder.AddBlockAllMixedContent();
                cspBuilder.AddImgSrc().Self();
                cspBuilder.AddFormAction().Self();
                cspBuilder.AddFontSrc().Self();
                cspBuilder.AddStyleSrc().Self();
                cspBuilder.AddBaseUri().Self();
                cspBuilder.AddFrameAncestors().None();
            })
            .AddPermissionsPolicy(builder =>
            {
                builder.AddAccelerometer().None();
                builder.AddAutoplay().None();
                builder.AddCamera().None();
                builder.AddEncryptedMedia().None();
                builder.AddFullscreen().All();
                builder.AddGeolocation().None();
                builder.AddGyroscope().None();
                builder.AddMagnetometer().None();
                builder.AddMicrophone().None();
                builder.AddMidi().None();
                builder.AddPayment().None();
                builder.AddPictureInPicture().None();
                builder.AddSyncXHR().None();
                builder.AddUsb().None();
            })
            .AddCrossOriginOpenerPolicy(builder => { builder.SameOrigin(); })
            .AddCrossOriginEmbedderPolicy(builder => { builder.RequireCorp(); })
            .AddCrossOriginResourcePolicy(builder => { builder.SameOrigin(); });
    }

    private static void AddDependenciesTo(WebApplicationBuilder builder)
    {
        
        builder.Services.AddScoped<ITrustSearch, TrustSearch>();
        builder.Services.AddScoped<ITrustProvider, TrustProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, HeaderRequirementHandler>();
        builder.Services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();
        builder.Services.AddHttpClient("AcademiesApi", (provider, httpClient) =>
        {
            var academiesApiOptions = provider.GetRequiredService<IOptions<AcademiesApiOptions>>();
            httpClient.BaseAddress = new Uri(academiesApiOptions.Value.Endpoint!);
            httpClient.DefaultRequestHeaders.Add("ApiKey", academiesApiOptions.Value.Key);
        });

        builder.Services.AddHttpContextAccessor();
    }

    private static void AddEnvironmentVariablesTo(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsLocalDevelopment())
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

        builder.Services.AddOptions<AcademiesApiOptions>()
            .Bind(builder.Configuration.GetSection(AcademiesApiOptions.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddAuthenticationServices(WebApplicationBuilder builder)
    {
        if (ShouldSkipAuthentication(builder))
            return;

        builder.Services.AddAuthorization(options =>
        {
            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = policyBuilder.Build();
            options.FallbackPolicy = options.DefaultPolicy;
        });
        builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration);
        builder.Services.Configure<CookieAuthenticationOptions>(
            CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.Cookie.Name = ".FindInformationAcademiesTrusts.Login";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
    }

    private static bool ShouldSkipAuthentication(WebApplicationBuilder builder)
    {
       if (!builder.Environment.IsLocalDevelopment() && !builder.Environment.IsContinuousIntegration())
            return false;

       // We need to be sure that this is actually an isolated environment with no access to production data
        var academiesApiUrl = builder.Configuration.GetSection("AcademiesApi").GetValue<string>("Endpoint")?.ToLower();
        return string.IsNullOrWhiteSpace(academiesApiUrl)
               || academiesApiUrl.Contains("localhost")
              || academiesApiUrl.Contains("wiremock");
    }

    private static void ReconfigureLogging(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsLocalDevelopment() || builder.Environment.IsContinuousIntegration())
        {
            builder.Host.UseSerilog((_, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console());
        }
        else
        {
            builder.Host.UseSerilog((_, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Traces));
        }
    }
}
