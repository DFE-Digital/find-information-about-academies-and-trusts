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

    public Task<DataSource> GetGiasUpdated()
    {
        return GetDataSourceFromApplicationEvents("GIAS_Daily", Source.Gias, UpdateFrequency.Daily);
    }

    public Task<DataSource> GetMstrUpdated()
    {
        return GetDataSourceFromApplicationEvents("MSTR_Daily", Source.Mstr, UpdateFrequency.Daily);
    }

    public Task<DataSource> GetCdmUpdated()
    {
        return GetDataSourceFromApplicationEvents("CDM_Daily", Source.Cdm, UpdateFrequency.Daily);
    }

    private async Task<DataSource> GetDataSourceFromApplicationEvents(string pipelineName, Source source,
        UpdateFrequency updateFrequency)
    {
        var lastEntry = await _academiesDbContext.ApplicationEvents
            .Where(e => e.Message == "Finished"
                        && e.EventType != 'E'
                        && e.Description == pipelineName).MaxAsync(e => e.DateTime);
        if (lastEntry is null)
        {
            _logger.LogError("Unable to find when {pipelineName} was last run", pipelineName);
            return new DataSource(source, null, updateFrequency);
        }

        return new DataSource(source, lastEntry.Value, updateFrequency);
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
