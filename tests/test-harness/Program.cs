using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.Setup;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace test_harness;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        //Create logging mechanism before anything else to catch bootstrap errors
        Log.Logger = LoggingSetup.CreateInitialLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = @"Z:\fiat\DfE.FindInformationAcademiesTrusts\",
                EnvironmentName = EnvironmentExtensions.ContinuousIntegrationEnvironmentName
            });

            builder.Host.UseSerilog((_, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console());

            ConfigurationVariables.BindConfigurationVariables(builder);

            builder.Services.AddRazorPages();
            //  builder.Services.AddApplicationInsightsTelemetry(); //This is a test harness, no app insights
            //    SecurityServicesSetup.AddSecurityServices(builder); //This is a test harness, no security required

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            Dependencies.AddDependenciesTo(builder);

            AddStubbedServices(builder);

            HealthCheckSetup.AddHealthChecks(builder);

            var app = builder.Build();
            PostBuildSetup.ConfigureApp(app);
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

    private static void AddStubbedServices(WebApplicationBuilder builder)
    {
        builder.Services.RemoveAll<ITrustSearch>();
        builder.Services.AddScoped<ITrustSearch, StubTrustSearch>();

        RemoveFiatServices(builder.Services);

        builder.Services.AddScoped<IDataSourceService, StubDataSourceService>();
        builder.Services.AddScoped<ITrustService, StubTrustService>();
        builder.Services.AddScoped<IAcademyService, StubAcademyService>();
        // builder.Services.AddScoped<IExportService>(_ => Mock.Of<IExportService>());
    }

    private static void RemoveFiatServices(IServiceCollection services)
    {
        foreach (var serviceInterface in FiatServiceInterfaces)
        {
            services.RemoveAll(serviceInterface);
        }
    }

    private static IEnumerable<Type> FiatServiceInterfaces { get; } = GetFiatServiceInterfaces();

    private static IEnumerable<Type> GetFiatServiceInterfaces()
    {
        var assembly = typeof(Program).Assembly;
        var serviceInterfaces = assembly.GetTypes()
            .Where(t => t is
            {
                IsInterface: true,
                IsPublic: true,
                Namespace: not null
            } && t.Namespace.StartsWith("DfE.FindInformationAcademiesTrusts.Services"));
        return serviceInterfaces;
    }
}
