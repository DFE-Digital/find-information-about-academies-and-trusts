using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class DataSourceProvider : IDataSourceProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;

    [ExcludeFromCodeCoverage]
    public DataSourceProvider(AcademiesDbContext academiesDbContext) : this(
        (IAcademiesDbContext)academiesDbContext)
    {
    }

    public DataSourceProvider(IAcademiesDbContext academiesDbContext)
    {
        _academiesDbContext = academiesDbContext;
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
            return new DataSource(Source.Mis, null, UpdateFrequency.Monthly);
        }

        return new DataSource(Source.Mis,
            lastEntry.Modified.Value, UpdateFrequency.Monthly);
    }

    public async Task<DataSource> GetMisFurtherEducationEstablishmentsUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationSettings
            .FirstOrDefaultAsync(e => e.Key == "ManagementInformationFurtherEducationSchoolTableData CSV Filename");
        if (lastEntry is null || lastEntry.Modified is null)
        {
            return new DataSource(Source.Mis, null, UpdateFrequency.Monthly);
        }

        return new DataSource(Source.Mis,
            lastEntry.Modified.Value, UpdateFrequency.Monthly);
    }
}
