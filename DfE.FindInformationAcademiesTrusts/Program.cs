using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Setup;
using Serilog;

namespace DfE.FindInformationAcademiesTrusts;

[ExcludeFromCodeCoverage]
internal static class Program
{
    public static void Main(string[] args)
    {
        //Create logging mechanism before anything else to catch bootstrap errors
        Log.Logger = LoggingSetup.CreateInitialLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            //Reconfigure logging before proceeding so any bootstrap exceptions can be written to App Insights
            LoggingSetup.ReconfigureLogging(builder);

            ConfigurationVariables.BindConfigurationVariables(builder);

            builder.Services.AddRazorPages();
            builder.Services.AddApplicationInsightsTelemetry();
            SecurityServicesSetup.AddSecurityServices(builder);

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            Dependencies.AddDependenciesTo(builder);
            HealthCheckSetup.AddHealthChecks(builder);

            var app = builder.Build();
            PostBuildSetup.ConfigureApp(app);
            app.UseAzureAppConfiguration();
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
}
