using System.Diagnostics.CodeAnalysis;
using DfE.FIAT.Data;
using DfE.FIAT.Data.AcademiesDb;
using DfE.FIAT.Data.AcademiesDb.Contexts;
using DfE.FIAT.Data.AcademiesDb.Repositories;
using DfE.FIAT.Data.FiatDb.Contexts;
using DfE.FIAT.Data.FiatDb.Repositories;
using DfE.FIAT.Data.Hardcoded;
using DfE.FIAT.Data.Repositories.Academy;
using DfE.FIAT.Data.Repositories.DataSource;
using DfE.FIAT.Data.Repositories.Trust;
using DfE.FIAT.Web.Pages;
using DfE.FIAT.Web.Services.Academy;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Export;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.EntityFrameworkCore;

namespace DfE.FIAT.Web.Setup;

[ExcludeFromCodeCoverage]
public static class Dependencies
{
    public static void AddDependenciesTo(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AcademiesDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AcademiesDb") ??
                                 throw new InvalidOperationException("Connection string 'AcademiesDb' not found."))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)); // Academies db data is always readonly;
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

        builder.Services.AddScoped<IStringFormattingUtilities, StringFormattingUtilities>();

        builder.Services.AddScoped<IAcademyRepository, AcademyRepository>();
        builder.Services.AddScoped<ITrustRepository, TrustRepository>();
        builder.Services.AddScoped<IDataSourceRepository, DataSourceRepository>();
        builder.Services.AddScoped<IContactRepository, ContactRepository>();

        builder.Services.AddScoped<IDataSourceService, DataSourceService>();
        builder.Services.AddScoped<ITrustService, TrustService>();
        builder.Services.AddScoped<IAcademyService, AcademyService>();
        builder.Services.AddScoped<IExportService, ExportService>();

        builder.Services.AddScoped<IOtherServicesLinkBuilder, OtherServicesLinkBuilder>();
        builder.Services.AddScoped<IFreeSchoolMealsAverageProvider, FreeSchoolMealsAverageProvider>();
        builder.Services.AddHttpContextAccessor();
    }
}
