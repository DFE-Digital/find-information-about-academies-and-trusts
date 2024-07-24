using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel(
    ITrustProvider trustProvider,
    IDataSourceProvider dataSourceProvider,
    IOtherServicesLinkBuilder otherServicesLinkBuilder,
    ILogger<DetailsModel> logger)
    : TrustsAreaModel(trustProvider, dataSourceProvider, logger, "Details")
{
    public TrustDetailsDto TrustDetailsDto { get; set; } = default!;
    public string? CompaniesHouseLink { get; set; }
    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? SchoolsFinancialBenchmarkingLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustDetailsDto = await TrustProvider.GetTrustDetailsAsync(Uid);

        CompaniesHouseLink = otherServicesLinkBuilder.CompaniesHouseListingLink(TrustDetailsDto);
        GetInformationAboutSchoolsLink =
            otherServicesLinkBuilder.GetInformationAboutSchoolsListingLink(TrustDetailsDto);
        SchoolsFinancialBenchmarkingLink =
            otherServicesLinkBuilder.SchoolFinancialBenchmarkingServiceListingLink(TrustDetailsDto);
        FindSchoolPerformanceLink = otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(TrustDetailsDto);

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new List<string> { "Trust details", "Reference numbers" }));

        return pageResult;
    }
}
