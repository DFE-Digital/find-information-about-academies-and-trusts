using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;

public class OverviewAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<OverviewAreaModel> logger)
    : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { PageName = "Overview" };
    public TrustOverviewServiceModel TrustOverview { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel("Trust details", "./TrustDetails", Uid, TrustPageMetadata.PageName!,
                this is TrustDetailsModel),
            new TrustSubNavigationLinkModel("Trust summary", "./TrustSummary", Uid, TrustPageMetadata.PageName!,
                this is TrustSummaryModel),
            new TrustSubNavigationLinkModel("Reference numbers", "./ReferenceNumbers", Uid, TrustPageMetadata.PageName!,
                this is ReferenceNumbersModel)
        ];

        // Fetch the trust overview data
        TrustOverview = await TrustService.GetTrustOverviewAsync(Uid);

        // Add data sources
        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);
        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry("Trust details", [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry("Trust summary", [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry("Reference numbers", [new DataSourceListEntry(giasDataSource)])
        ]);

        return Page();
    }
}
