using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FIAT.Web.Pages.Trusts.Overview;

public class OverviewAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<OverviewAreaModel> logger)
    : TrustsAreaModel(dataSourceService, trustService, logger, "Overview")
{
    public TrustOverviewServiceModel TrustOverview { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel("Trust details", "./TrustDetails", Uid, PageName,
                this is TrustDetailsModel),
            new TrustSubNavigationLinkModel("Trust summary", "./TrustSummary", Uid, PageName,
                this is TrustSummaryModel),
            new TrustSubNavigationLinkModel("Reference numbers", "./ReferenceNumbers", Uid, PageName,
                this is ReferenceNumbersModel)
        ];

        // Fetch the trust overview data
        TrustOverview = await TrustService.GetTrustOverviewAsync(Uid);

        // Add data sources
        DataSources.Add(new DataSourceListEntry(
            await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Trust details", "Trust summary", "Reference numbers" }));

        return Page();
    }
}
