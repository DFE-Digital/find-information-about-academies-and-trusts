using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FIAT.Web.Pages.Trusts;

public class OverviewModel(
    IDataSourceService dataSourceService,
    ILogger<OverviewModel> logger,
    ITrustService trustService,
    IOtherServicesLinkBuilder otherServicesLinkBuilder)
    : TrustsAreaModel(dataSourceService, trustService, logger, "Overview")
{
    public TrustOverviewServiceModel TrustOverview { get; set; } = default!;
    public string? CompaniesHouseLink { get; set; }
    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? SchoolsFinancialBenchmarkingLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        // Fetch the trust overview data
        TrustOverview = await TrustService.GetTrustOverviewAsync(Uid);

        // Setup external links
        CompaniesHouseLink = otherServicesLinkBuilder.CompaniesHouseListingLink(TrustOverview.CompaniesHouseNumber);
        GetInformationAboutSchoolsLink =
            otherServicesLinkBuilder.GetInformationAboutSchoolsListingLinkForTrust(TrustOverview.Uid);
        SchoolsFinancialBenchmarkingLink =
            otherServicesLinkBuilder.SchoolFinancialBenchmarkingServiceListingLink(TrustOverview.Type,
                TrustOverview.SingleAcademyTrustAcademyUrn, TrustOverview.CompaniesHouseNumber);
        FindSchoolPerformanceLink =
            otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(TrustOverview.Uid, TrustOverview.Type,
                TrustOverview.SingleAcademyTrustAcademyUrn);

        // Add data sources
        DataSources.Add(new DataSourceListEntry(
            await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Trust details", "Trust summary", "Reference numbers" }));

        return Page();
    }

    public IEnumerable<(string Authority, int Total)> AcademiesInEachLocalAuthority =>
        TrustOverview.AcademiesByLocalAuthority
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .Select(kv => (Authority: kv.Key, Total: kv.Value));
}
