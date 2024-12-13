using System.Diagnostics.CodeAnalysis;
using DfE.FIAT.Data.AcademiesDb.Contexts;
using DfE.FIAT.Data.FiatDb.Contexts;

namespace DfE.FIAT.Web.Setup;

[ExcludeFromCodeCoverage]
public static class HealthCheckSetup
{
    public static void AddHealthChecks(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
        AddDbHealthChecks(builder);
    }

    public static void AddDbHealthChecks(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AcademiesDbContext>()
            .AddDbContextCheck<FiatDbContext>();
    }
}
