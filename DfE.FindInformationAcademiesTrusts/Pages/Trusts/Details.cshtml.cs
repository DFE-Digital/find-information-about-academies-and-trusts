using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    IOtherServicesLinkBuilder otherServicesLinkBuilder,
    ILogger<DetailsModel> logger,
    ITrustService trustService)
    : TrustsAreaModel(trustProvider, dataSourceService, trustService, logger, "Details")
{
    public TrustDetailsServiceModel TrustDetails { get; set; } = default!;
    public string? CompaniesHouseLink { get; set; }
    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? SchoolsFinancialBenchmarkingLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustDetails = await TrustService.GetTrustDetailsAsync(Uid);

        CompaniesHouseLink = otherServicesLinkBuilder.CompaniesHouseListingLink(TrustDetails);
        GetInformationAboutSchoolsLink =
            otherServicesLinkBuilder.GetInformationAboutSchoolsListingLinkForTrust(TrustDetails.Uid);
        SchoolsFinancialBenchmarkingLink =
            otherServicesLinkBuilder.SchoolFinancialBenchmarkingServiceListingLink(TrustDetails);
        FindSchoolPerformanceLink =
            otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(TrustDetails);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Trust details", "Reference numbers" }));

        return pageResult;
    }
}
