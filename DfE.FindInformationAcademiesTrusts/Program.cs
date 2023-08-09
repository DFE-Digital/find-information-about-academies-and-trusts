using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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
        if (!app.Environment.IsDevelopment() && !app.Environment.IsLocalDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always, HttpOnly = HttpOnlyPolicy.Always,
            MinimumSameSitePolicy = SameSiteMode.None
        });


        app.UseHttpsRedirection();

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
    }

    private static void AddDependenciesTo(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITrustSearch, TrustSearch>();
        builder.Services.AddScoped<ITrustProvider, TrustProvider>();

        builder.Services.AddHttpClient("AcademiesApi", (provider, httpClient) =>
        {
            var academiesApiOptions = provider.GetRequiredService<IOptions<AcademiesApiOptions>>();
            httpClient.BaseAddress = new Uri(academiesApiOptions.Value.Endpoint!);
            httpClient.DefaultRequestHeaders.Add("ApiKey", academiesApiOptions.Value.Key);
        });
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
        if (!builder.Environment.IsLocalDevelopment() && !builder.Environment.IsContinuousIntegration())
        {
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
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Host.UseSerilog((_, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Traces));
        }
    }
}
