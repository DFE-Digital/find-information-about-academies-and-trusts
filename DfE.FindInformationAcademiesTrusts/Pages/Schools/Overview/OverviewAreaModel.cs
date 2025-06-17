using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public abstract class OverviewAreaModel(
    ISchoolService schoolService,
    ITrustService trustService,
    IDataSourceService dataSourceService) : SchoolAreaModel(schoolService, trustService)
{
    public const string PageName = "Overview";
    public override PageMetadata PageMetadata => base.PageMetadata with { PageName = PageName };

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        // Add data sources
        var giasDataSource = await dataSourceService.GetAsync(Source.Gias);

        DataSourcesPerPage = GetDataSources(giasDataSource, SchoolCategory);

        return Page();
    }

    private List<DataSourcePageListEntry> GetDataSources(DataSourceServiceModel giasDataSource,
        SchoolCategory schoolCategory)
    {
        var dataSources = new List<DataSourcePageListEntry>
        {
            new(DetailsModel.SubPageName(schoolCategory), [new DataSourceListEntry(giasDataSource)]),
            new(SenModel.SubPageName, [new DataSourceListEntry(giasDataSource)])
        };

        if (schoolCategory == SchoolCategory.LaMaintainedSchool)
        {
            dataSources.Insert(1,
                new DataSourcePageListEntry(FederationModel.SubPageName, [new DataSourceListEntry(giasDataSource)]));
        }

        return dataSources;
    }
}
