using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel(
    ITrustProvider trustProvider,
    IDataSourceProvider dataSourceProvider,
    IOtherServicesLinkBuilder otherServicesLinkBuilder,
    ILogger<DetailsModel> logger)
    : TrustsAreaModel(trustProvider, dataSourceProvider, logger, "Details")
{
    public Trust Trust { get; set; } = default!;
    public string? CompaniesHouseLink { get; set; }
    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? SchoolsFinancialBenchmarkingLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Trust = (await TrustProvider.GetTrustByUidAsync(Uid))!;

        CompaniesHouseLink = otherServicesLinkBuilder.CompaniesHouseListingLink(Trust);
        GetInformationAboutSchoolsLink = otherServicesLinkBuilder.GetInformationAboutSchoolsListingLink(Trust);
        SchoolsFinancialBenchmarkingLink =
            otherServicesLinkBuilder.SchoolFinancialBenchmarkingServiceListingLink(Trust);
        FindSchoolPerformanceLink = otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(Trust);

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new List<string> { "Trust details", "Reference numbers" }));

        return pageResult;
    }
}
