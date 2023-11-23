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

    public DetailsModel(ITrustProvider trustProvider, IOtherServicesLinkBuilder otherServicesLinkBuilder) : base(
        trustProvider,
        "Details")
    {
        _otherServicesLinkBuilder = otherServicesLinkBuilder;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        CompaniesHouseLink = _otherServicesLinkBuilder.CompaniesHouseListingLink(Trust);
        GetInformationAboutSchoolsLink = _otherServicesLinkBuilder.GetInformationAboutSchoolsListingLink(Trust);
        SchoolsFinancialBenchmarkingLink =
            _otherServicesLinkBuilder.SchoolFinancialBenchmarkingServiceListingLink(Trust);
        FindSchoolPerformanceLink = _otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(Trust);

        return pageResult;
    }
}
