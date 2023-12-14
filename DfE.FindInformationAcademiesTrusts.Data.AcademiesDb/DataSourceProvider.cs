using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class DataSourceProvider : IDataSourceProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ILogger<DataSourceProvider> _logger;

    [ExcludeFromCodeCoverage]
    public DataSourceProvider(AcademiesDbContext academiesDbContext, ILogger<DataSourceProvider> logger) : this(
        (IAcademiesDbContext)academiesDbContext, logger)
    {
    }

    public DataSourceProvider(IAcademiesDbContext academiesDbContext, ILogger<DataSourceProvider> logger)
    {
        _academiesDbContext = academiesDbContext;
        _logger = logger;
    }

    public async Task<DataSource> GetGiasUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationEvents
            .Where(e => e.Source != null
                        && e.Source.Contains("adf-t1") && e.Source.Contains("-sips-dataflow")
                        && e.Message == "Finished"
                        && e.EventType != 'E'
                        && e.Description == "GIAS_Daily").MaxAsync(e => e.DateTime);
        if (lastEntry is null)
        {
            _logger.LogError("Unable to find when GIAS pipeline was last run");
            return new DataSource(Source.Gias, null, UpdateFrequency.Daily);
        }

        return new DataSource(Source.Gias, lastEntry.Value, UpdateFrequency.Daily);
    }

    public async Task<DataSource> GetMstrUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationEvents
            .Where(e => e.Source != null
                        && e.Source.Contains("adf-t1") && e.Source.Contains("-sips-dataflow")
                        && e.Message == "Finished"
                        && e.EventType != 'E'
                        && e.Description == "MSTR_Daily").MaxAsync(e => e.DateTime);
        if (lastEntry is null)
        {
            _logger.LogError("Unable to find when MSTR pipeline was last run");
            return new DataSource(Source.Mstr, null, UpdateFrequency.Daily);
        }

        return new DataSource(Source.Mstr, lastEntry.Value, UpdateFrequency.Daily);
    }

    public async Task<DataSource> GetCdmUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationEvents
            .Where(e => e.Source != null
                        && e.Source.Contains("adf-t1") && e.Source.Contains("-sips-dataflow")
                        && e.Message == "Finished"
                        && e.EventType != 'E'
                        && e.Description == "CDM_Daily").MaxAsync(e => e.DateTime);
        if (lastEntry is null)
        {
            _logger.LogError("Unable to find when CDM pipeline was last run");
            return new DataSource(Source.Cdm, null, UpdateFrequency.Daily);
        }

        return new DataSource(Source.Cdm, lastEntry.Value, UpdateFrequency.Daily);
    }

    public async Task<DataSource> GetMisEstablishmentsUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationSettings
            .FirstOrDefaultAsync(e => e.Key == "ManagementInformationSchoolTableData CSV Filename");
        if (lastEntry is null || lastEntry.Modified is null)
        {
            _logger.LogError("Unable to find when ManagementInformationSchoolTableData was last modified");
            return new DataSource(Source.Mis, null, UpdateFrequency.Monthly);
        }

        return new DataSource(Source.Mis,
            lastEntry.Modified.Value, UpdateFrequency.Monthly);
    }
}
