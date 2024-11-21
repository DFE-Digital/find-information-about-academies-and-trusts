using System.Diagnostics.CodeAnalysis;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Extensions.Hosting;

namespace DfE.FindInformationAcademiesTrusts.Setup;

[ExcludeFromCodeCoverage]
public static class LoggingSetup
{
    public static void ReconfigureLogging(WebApplicationBuilder builder)
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

    public static ReloadableLogger CreateInitialLogger()
    {
        return new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }
}
