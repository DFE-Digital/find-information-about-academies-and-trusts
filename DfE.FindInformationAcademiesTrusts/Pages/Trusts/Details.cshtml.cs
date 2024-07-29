using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel(
    ITrustProvider trustProvider,
    IDataSourceProvider dataSourceProvider,
    IOtherServicesLinkBuilder otherServicesLinkBuilder,
    ILogger<DetailsModel> logger,
    ITrustService trustService)
    : TrustsAreaModel(trustProvider, dataSourceProvider, trustService, logger, "Details")
{
    public TrustDetailsServiceModel TrustDetailsServiceModel { get; set; } = default!;
    public string? CompaniesHouseLink { get; set; }
    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? SchoolsFinancialBenchmarkingLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustDetailsServiceModel = await TrustService.GetTrustDetailsAsync(Uid);

        CompaniesHouseLink = otherServicesLinkBuilder.CompaniesHouseListingLink(TrustDetailsServiceModel);
        GetInformationAboutSchoolsLink =
            otherServicesLinkBuilder.GetInformationAboutSchoolsListingLink(TrustDetailsServiceModel);
        SchoolsFinancialBenchmarkingLink =
            otherServicesLinkBuilder.SchoolFinancialBenchmarkingServiceListingLink(TrustDetailsServiceModel);
        FindSchoolPerformanceLink =
            otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(TrustDetailsServiceModel);

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new List<string> { "Trust details", "Reference numbers" }));

        return pageResult;
    }
}
