using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
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
            if (builder.Environment.IsLocalDevelopment())
                builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

            //Reconfigure logging before proceeding so any bootstrap exceptions can be written to App Insights 
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

            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = SetupAuthorizationPolicyBuilder().Build();
                options.FallbackPolicy = options.DefaultPolicy;
            });
            builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration);
            // Add services to the container.
            builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.Cookie.Name = ".FindInformationAcademiesTrusts.Login";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });


            builder.Services.AddRazorPages().AddMicrosoftIdentityUI();

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ITrustSearch, TrustSearch>();
            builder.Services.AddScoped<ITrustProvider, TrustProvider>();
            builder.Services.AddOptions<AcademiesApiOptions>()
                .Bind(builder.Configuration.GetSection(AcademiesApiOptions.ConfigurationSection))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddHttpClient("AcademiesApi", (provider, httpClient) =>
            {
                var academiesApiOptions = provider.GetRequiredService<IOptions<AcademiesApiOptions>>();
                httpClient.BaseAddress = new Uri(academiesApiOptions.Value.Endpoint!);
                httpClient.DefaultRequestHeaders.Add("ApiKey", academiesApiOptions.Value.Key);
            });

            //Build and configure app
            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.UseMiddleware<ResponseHeadersMiddleware>();

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

    private static AuthorizationPolicyBuilder SetupAuthorizationPolicyBuilder()
    {
        var policyBuilder = new AuthorizationPolicyBuilder();
        policyBuilder.RequireAuthenticatedUser();

        return policyBuilder;
    }
}
