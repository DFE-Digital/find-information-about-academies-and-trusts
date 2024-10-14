using System.Data;
using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using DfE.FindInformationAcademiesTrusts.Authorization;
using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Identity.Web;

namespace DfE.FindInformationAcademiesTrusts.Setup;

[ExcludeFromCodeCoverage]
public static class SecurityServicesSetup
{
    public static void AddSecurityServices(WebApplicationBuilder builder)
    {
        AddHsts(builder);
        AddIdentityServices(builder);
        AddAntiForgeryCookies(builder);
        AddDataProtectionServices(builder);
    }

    private static void AddHsts(WebApplicationBuilder builder)
    {
        // Enforce HTTPS in ASP.NET Core
        // @link https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?
        builder.Services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });
    }

    private static void AddIdentityServices(WebApplicationBuilder builder)
    {
        // Setup bypass for automation tests
        builder.Services.AddScoped<IAuthorizationHandler, AutomationAuthorizationHandler>();

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
                options.Cookie.Name = FiatCookies.Login;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                options.AccessDeniedPath = "/no-access";
            });
    }

    private static void AddAntiForgeryCookies(WebApplicationBuilder builder)
    {
        builder.Services.AddAntiforgery(opts => { opts.Cookie.Name = FiatCookies.Antiforgery; });
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
}
