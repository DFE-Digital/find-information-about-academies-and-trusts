using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel : TrustsAreaModel
{
    private readonly IOtherServicesLinkBuilder _otherServicesLinkBuilder;
    public string? CompaniesHouseLink { get; set; }
    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? SchoolsFinancialBenchmarkingLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public DetailsModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider,
        IOtherServicesLinkBuilder otherServicesLinkBuilder, ILogger<DetailsModel> logger) : base(
        trustProvider, dataSourceProvider, logger, "Details")
    {
        _otherServicesLinkBuilder = otherServicesLinkBuilder;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        CompaniesHouseLink = _otherServicesLinkBuilder.CompaniesHouseListingLink(Trust);
        GetInformationAboutSchoolsLink = _otherServicesLinkBuilder.GetInformationAboutSchoolsListingLink(Trust);
        SchoolsFinancialBenchmarkingLink =
            _otherServicesLinkBuilder.SchoolFinancialBenchmarkingServiceListingLink(Trust);
        FindSchoolPerformanceLink = _otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(Trust);

        DataSources.Add(new DataSourceListEntry(await DataSourceProvider.GetGiasUpdated(),
            new List<string> { "Trust details", "Reference numbers" }));

        return pageResult;
    }
}
