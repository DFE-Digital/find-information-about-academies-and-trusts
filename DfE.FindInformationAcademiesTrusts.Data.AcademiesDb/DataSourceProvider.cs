﻿using System.Diagnostics.CodeAnalysis;
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

    public async Task<DataSource?> GetGIASUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationEvents
            .Where(e => e.Source != null
                        && EF.Functions.Like(e.Source, "adf-t1__-sips-dataflow")
                        && e.Message == "Finished"
                        && e.EventType != 'E'
                        && e.Description == "GIAS_Daily").MaxAsync(e => e.DateTime);
        if (lastEntry is null) return null;
        return new DataSource("Get Information about Schools", lastEntry.Value, lastEntry.Value.AddDays(1), "Daily");
    }

    public async Task<DataSource?> GetMSTRUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationEvents
            .Where(e => e.Source != null
                       && EF.Functions.Like(e.Source, "adf-t1__-sips-dataflow")
                       && e.Message == "Finished"
                       && e.EventType != 'E'
                       && e.Description == "MSTR_Daily").MaxAsync(e => e.DateTime);
        if (lastEntry is null) return null;
        return new DataSource("MSTR", lastEntry.Value, lastEntry.Value.AddDays(1), "Daily");
    }

    public async Task<DataSource?> GetCDMUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationEvents
            .Where(e => e.Source != null
                        && EF.Functions.Like(e.Source, "adf-t1__-sips-dataflow")
                        && e.Message == "Finished"
                        && e.EventType != 'E'
                        && e.Description == "CDM_Daily").MaxAsync(e => e.DateTime);
        if (lastEntry is null) return null;
        return new DataSource("CDM", lastEntry.Value, lastEntry.Value.AddDays(1), "Daily");
    }

    public async Task<DataSource?> GetMISEstablishmentsUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationSettings
            .FirstOrDefaultAsync(e => e.Key == "ManagementInformationSchoolTableData CSV Filename");
        if (lastEntry is null || lastEntry.Modified is null) return null;
        return new DataSource("MISEstablishments", lastEntry.Modified.Value,
            lastEntry.Modified.Value.AddMonths(1), "Monthly");
    }

    public async Task<DataSource?> GetMISFurtherEducationEstablishmentsUpdated()
    {
        var lastEntry = await _academiesDbContext.ApplicationSettings
            .FirstOrDefaultAsync(e => e.Key == "ManagementInformationFurtherEducationSchoolTableData CSV Filename");
        if (lastEntry is null || lastEntry.Modified is null) return null;
        return new DataSource("MISFurtherEducationEstablishments", lastEntry.Modified.Value,
            lastEntry.Modified.Value.AddMonths(1), "Monthly");
    }
}
