using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public class DetailsModel(ISchoolService schoolService, ITrustService trustService, ISchoolOverviewDetailsService schoolOverviewDetailsService, IOtherServicesLinkBuilder otherServicesLinkBuilder, IDataSourceService dataSourceService) : OverviewAreaModel(schoolService, trustService, dataSourceService)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = SubPageName(SchoolCategory)
    };

    public static string SubPageName(SchoolCategory schoolCategory)
    {
        return schoolCategory switch
        {
            SchoolCategory.LaMaintainedSchool => "School details",
            SchoolCategory.Academy => "Academy details",
            _ => throw new ArgumentOutOfRangeException(nameof(schoolCategory))
        };
    }

    public SchoolOverviewServiceModel SchoolOverviewModel { get; private set; } = null!;

    public string? GetInformationAboutSchoolsLink { get; set; }
    public string? FinancialBenchmarkingInsightsToolLink { get; set; }
    public string? FindSchoolPerformanceLink { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        var schoolOverviewDetails = await schoolOverviewDetailsService.GetSchoolOverviewDetailsAsync(Urn, SchoolCategory);

        if (schoolOverviewDetails is null)
        {
            return new NotFoundResult();
        }

        SchoolOverviewModel = schoolOverviewDetails;

        GetInformationAboutSchoolsLink = otherServicesLinkBuilder.GetInformationAboutSchoolsListingLinkForSchool(Urn.ToString());
        FinancialBenchmarkingInsightsToolLink = otherServicesLinkBuilder.FinancialBenchmarkingLinkForSchool(Urn);
        FindSchoolPerformanceLink = otherServicesLinkBuilder.FindSchoolPerformanceDataListingLink(Urn);
        return pageResult;
    }
}
