using Azure.Identity;
using DfE.FindInformationAcademiesTrusts.Authorization;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Hardcoded;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.DataSource;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Options;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.Identity.Web;
using Serilog;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

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

            // Enforce HTTPS in ASP.NET Core
            // @link https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

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

            AddDataProtectionServices(builder);

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
        // Ensure we do not lose X-Forwarded-* Headers when behind a Proxy
        var forwardOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All,
            RequireHeaderSymmetry = false
        };
        forwardOptions.KnownNetworks.Clear();
        forwardOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardOptions);

        // Set HTTP Security headers
        app.UseSecurityHeaders(GetSecurityHeaderPolicies());

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
    }

    private static HeaderPolicyCollection GetSecurityHeaderPolicies()
    {
        return new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddXssProtectionDisabled()
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
                    .From(new[]
                    {
                        "https://js.monitor.azure.com/scripts/b/ai.2.min.js",
                        "https://js.monitor.azure.com/scripts/b/ai.3.gbl.min.js",
                        "https://js.monitor.azure.com/scripts/b/ext/ai.clck.2.8.18.min.js",
                        "https://www.googletagmanager.com"
                    });
                cspBuilder.AddConnectSrc()
                    .Self()
                    .From(new[]
                    {
                        "https://*.in.applicationinsights.azure.com//v2/track",
                        "https://*.in.applicationinsights.azure.com/v2/track",
                        "https://js.monitor.azure.com/scripts/b/ai.config.1.cfg.json",
                        "https://*.google-analytics.com"
                    });
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
        builder.Services.AddDbContext<AcademiesDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AcademiesDb") ??
                                 throw new InvalidOperationException("Connection string 'AcademiesDb' not found.")));
        builder.Services.AddScoped<IAcademiesDbContext>(provider =>
            provider.GetService<AcademiesDbContext>() ??
            throw new InvalidOperationException("AcademiesDbContext not registered"));

        builder.Services.AddDbContext<FiatDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                                 throw new InvalidOperationException(
                                     "FIAT database connection string 'DefaultConnection' not found.")));

        builder.Services.AddScoped<SetChangedByInterceptor>();
        builder.Services.AddScoped<IUserDetailsProvider, HttpContextUserDetailsProvider>();

        builder.Services.AddScoped<ITrustSearch, TrustSearch>();

        builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        builder.Services.AddScoped<IAcademyRepository, AcademyRepository>();
        builder.Services.AddScoped<ITrustRepository, TrustRepository>();
        builder.Services.AddScoped<IDataSourceRepository, DataSourceRepository>();
        builder.Services.AddScoped<IContactRepository, ContactRepository>();

        builder.Services.AddScoped<IDataSourceService, DataSourceService>();
        builder.Services.AddScoped<ITrustService, TrustService>();
        builder.Services.AddScoped<IAcademyService, AcademyService>();
        builder.Services.AddScoped<IExportService, ExportService>();

        builder.Services.AddScoped<IAuthorizationHandler, AutomationAuthorizationHandler>();
        builder.Services.AddScoped<IOtherServicesLinkBuilder, OtherServicesLinkBuilder>();
        builder.Services.AddScoped<IFreeSchoolMealsAverageProvider, FreeSchoolMealsAverageProvider>();
        builder.Services.AddHttpContextAccessor();
    }

    private static void AddEnvironmentVariablesTo(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsLocalDevelopment())
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
        builder.Services.AddOptions<TestOverrideOptions>()
            .Bind(builder.Configuration.GetSection(TestOverrideOptions.ConfigurationSection));
        builder.Services.AddOptions<ApplicationInsightsOptions>()
            .Bind(builder.Configuration.GetSection(ApplicationInsightsOptions.ConfigurationSection));
        builder.Services.AddOptions<NotificationBannerOptions>()
            .Bind(builder.Configuration.GetSection(NotificationBannerOptions.ConfigurationSection));
        builder.Services.AddFeatureManagement();
    }

    private static void AddAuthenticationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("User.Role.Authorised")
                .Build();
            options.FallbackPolicy = options.DefaultPolicy;
        });

        if (!builder.Environment.IsContinuousIntegration())
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

                options.AccessDeniedPath = "/no-access";
            });

        builder.Services.AddAntiforgery(opts => { opts.Cookie.Name = ".FindInformationAcademiesTrusts.Antiforgery"; });
    }

    private static void AddDataProtectionServices(WebApplicationBuilder builder)
    {
        // Setup basic Data Protection and persist keys.xml to local file system
        var dp = builder.Services.AddDataProtection();

        // If a Key Vault Key URI is defined, expect to encrypt the keys.xml
        var kvProtectionKeyUri = builder.Configuration.GetValue<string>("DataProtection:KeyVaultKey");
        if (!string.IsNullOrWhiteSpace(kvProtectionKeyUri))
        {
            var kvProtectionPath = builder.Configuration.GetValue<string>("DataProtection:Path");

            if (string.IsNullOrWhiteSpace(kvProtectionPath))
            {
                throw new InvalidOperationException("DataProtection:Path is undefined or empty");
            }

            var kvProtectionPathDir = new DirectoryInfo(kvProtectionPath);
            if (!kvProtectionPathDir.Exists || kvProtectionPathDir.Attributes.HasFlag(FileAttributes.ReadOnly))
            {
                throw new ReadOnlyException($"DataProtection path '{kvProtectionPath}' cannot be written to");
            }

            dp.PersistKeysToFileSystem(kvProtectionPathDir);
            dp.ProtectKeysWithAzureKeyVault(new Uri(kvProtectionKeyUri), new DefaultAzureCredential());
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
            builder.Host.UseSerilog((_, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Traces));
        }
    }
}
