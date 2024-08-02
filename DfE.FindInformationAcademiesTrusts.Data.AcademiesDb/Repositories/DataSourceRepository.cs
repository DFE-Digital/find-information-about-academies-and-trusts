using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class DataSourceRepository(
    IAcademiesDbContext academiesDbContext,
    ILogger<DataSourceRepository> logger)
    : IDataSourceRepository
{
    public async Task<DataSource> GetGiasUpdatedAsync()
    {
        return await GetDataSourceFromApplicationEvents("GIAS_Daily", Source.Gias, UpdateFrequency.Daily);
    }

    public async Task<DataSource> GetMstrUpdatedAsync()
    {
        return await GetDataSourceFromApplicationEvents("MSTR_Daily", Source.Mstr, UpdateFrequency.Daily);
    }

    public async Task<DataSource> GetCdmUpdatedAsync()
    {
        return await GetDataSourceFromApplicationEvents("CDM_Daily", Source.Cdm, UpdateFrequency.Daily);
    }

    public async Task<DataSource> GetMisEstablishmentsUpdatedAsync()
    {
        var lastEntry = await academiesDbContext.ApplicationSettings
            .FirstOrDefaultAsync(e => e.Key == "ManagementInformationSchoolTableData CSV Filename");
        if (lastEntry?.Modified is null)
        {
            logger.LogError("Unable to find when ManagementInformationSchoolTableData was last modified");
            return new DataSource(Source.Mis, null, UpdateFrequency.Monthly);
        }

        return new DataSource(Source.Mis,
            lastEntry.Modified.Value, UpdateFrequency.Monthly);
    }

    private async Task<DataSource> GetDataSourceFromApplicationEvents(string pipelineName, Source source,
        UpdateFrequency updateFrequency)
    {
        var lastEntry = await academiesDbContext.ApplicationEvents
            .Where(e => e.Message == "Finished"
                        && e.EventType != 'E'
                        && e.Description == pipelineName).MaxAsync(e => e.DateTime);
        if (lastEntry is null)
        {
            logger.LogError("Unable to find when {pipelineName} was last run", pipelineName);
            return new DataSource(source, null, updateFrequency);
        }

        return new DataSource(source, lastEntry.Value, updateFrequency);
    }
}
