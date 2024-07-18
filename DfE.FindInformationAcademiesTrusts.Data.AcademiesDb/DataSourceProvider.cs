using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class DataSourceProvider : IDataSourceProvider
{
    private readonly IMemoryCache _memoryCache;
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ILogger<DataSourceProvider> _logger;
    //
    // [ExcludeFromCodeCoverage]
    // public DataSourceProvider(AcademiesDbContext academiesDbContext, ILogger<DataSourceProvider> logger,
    //     IMemoryCache memoryCache) : this(
    //     (IAcademiesDbContext)academiesDbContext, logger, memoryCache)
    // {
    //     _memoryCache = memoryCache;
    // }

    public DataSourceProvider(IAcademiesDbContext academiesDbContext, ILogger<DataSourceProvider> logger,
        IMemoryCache memoryCache)
    {
        _academiesDbContext = academiesDbContext;
        _logger = logger;
        _memoryCache = memoryCache;
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
        if (!_memoryCache.TryGetValue(source, out DateTime? lastEntry) || lastEntry is null)
        {
            lastEntry = await _academiesDbContext.ApplicationEvents
                .Where(e => e.Message == "Finished"
                            && e.EventType != 'E'
                            && e.Description == pipelineName).MaxAsync(e => e.DateTime);

            TimeSpan cacheExpiration;
            try
            {
                cacheExpiration = updateFrequency switch
                {
                    UpdateFrequency.Daily => TimeSpan.FromHours(1),
                    UpdateFrequency.Monthly => TimeSpan.FromDays(1),
                    UpdateFrequency.Annually => TimeSpan.FromDays(1),
                    _ => throw new ArgumentOutOfRangeException(nameof(updateFrequency), updateFrequency, null)
                };
            }
            catch (ArgumentOutOfRangeException)
            {
                _logger.LogError("Unknown update frequency: {updateFrequency}", updateFrequency);
                cacheExpiration = TimeSpan.FromHours(1);
            }

            _memoryCache.Set(source, lastEntry, cacheExpiration);
        }

        if (lastEntry is null)
        {
            _logger.LogError("Unable to find when {pipelineName} was last run", pipelineName);
            return new DataSource(source, null, updateFrequency);
        }

        return new DataSource(source, lastEntry.Value, updateFrequency);
    }

    public async Task<DataSource> GetMisEstablishmentsUpdated()
    {
        if (!_memoryCache.TryGetValue(Source.Mis, out ApplicationSetting? lastEntry) || lastEntry is null)
        {
            lastEntry = await _academiesDbContext.ApplicationSettings
                .FirstOrDefaultAsync(e => e.Key == "ManagementInformationSchoolTableData CSV Filename");

            _memoryCache.Set(Source.Mis, lastEntry, TimeSpan.FromDays(1));
        }

        if (lastEntry?.Modified is null)
        {
            _logger.LogError("Unable to find when ManagementInformationSchoolTableData was last modified");
            return new DataSource(Source.Mis, null, UpdateFrequency.Monthly);
        }

        return new DataSource(Source.Mis,
            lastEntry.Modified.Value, UpdateFrequency.Monthly);
    }
}
