using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FIAT.Web.Pages.Trusts.Overview;

public class TrustDetailsModel(
    IDataSourceService dataSourceService,
    ILogger<TrustDetailsModel> logger,
    ITrustService trustService,
    IOtherServicesLinkBuilder otherServicesLinkBuilder)
    : OverviewAreaModel(dataSourceService, trustService, logger)
{
    public string? CompaniesHouseLink { get; set; }
    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? FinancialBenchmarkingInsightsToolLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        // Setup external links
        CompaniesHouseLink = otherServicesLinkBuilder.CompaniesHouseListingLink(TrustOverview.CompaniesHouseNumber);
        GetInformationAboutSchoolsLink =
            otherServicesLinkBuilder.GetInformationAboutSchoolsListingLinkForTrust(TrustOverview.Uid);
        FinancialBenchmarkingInsightsToolLink =
            otherServicesLinkBuilder.FinancialBenchmarkingInsightsToolListingLink(TrustOverview.CompaniesHouseNumber);
        FindSchoolPerformanceLink =
            otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(TrustOverview.Uid, TrustOverview.Type,
                TrustOverview.SingleAcademyTrustAcademyUrn);

        return Page();
    }
}
