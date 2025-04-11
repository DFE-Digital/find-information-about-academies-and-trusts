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
    public const string PageName = "Overview";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { PageName = PageName };

    public TrustOverviewServiceModel TrustOverview { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel(ViewConstants.OverviewTrustDetailsPageName, "./TrustDetails", Uid, TrustPageMetadata.PageName!,
                this is TrustDetailsModel),
            new TrustSubNavigationLinkModel(ViewConstants.OverviewTrustSummaryPageName, "./TrustSummary", Uid, TrustPageMetadata.PageName!,
                this is TrustSummaryModel),
            new TrustSubNavigationLinkModel(ViewConstants.OverviewReferenceNumbersPageName, "./ReferenceNumbers", Uid, TrustPageMetadata.PageName!,
                this is ReferenceNumbersModel)
        ];

        // Fetch the trust overview data
        TrustOverview = await TrustService.GetTrustOverviewAsync(Uid);

        // Add data sources
        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);
        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry(TrustDetailsModel.SubPageName, [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(TrustSummaryModel.SubPageName, [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ReferenceNumbersModel.SubPageName, [new DataSourceListEntry(giasDataSource)])
        ]);

        return Page();
    }
}
