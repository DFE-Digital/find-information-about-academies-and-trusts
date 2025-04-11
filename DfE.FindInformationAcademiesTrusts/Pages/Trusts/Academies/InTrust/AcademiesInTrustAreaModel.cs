using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;

public abstract class AcademiesInTrustAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IAcademiesExportService academiesExportService,
    ILogger<AcademiesInTrustAreaModel> logger,
    IDateTimeProvider dateTimeProvider
) : AcademiesAreaModel(
    dataSourceService,
    trustService,
    academyService,
    logger,
    dateTimeProvider
)
{
    public const string SubPageName = "In this trust";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);
        var eesDataSource = await DataSourceService.GetAsync(Source.ExploreEducationStatistics);

        TabList =
        [
            new TrustTabNavigationLinkModel("Details", "./Details", "In this trust",
                this is AcademiesInTrustDetailsModel),
            new TrustTabNavigationLinkModel("Pupil numbers", "./PupilNumbers", "In this trust",
                this is PupilNumbersModel),
            new TrustTabNavigationLinkModel("Free school meals", "./FreeSchoolMeals", "In this trust",
                this is FreeSchoolMealsModel)
        ];

        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry(AcademiesInTrustDetailsModel.TabName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(PupilNumbersModel.TabName, [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(FreeSchoolMealsModel.TabName, [
                new DataSourceListEntry(giasDataSource, "Pupils eligible for free school meals"),
                new DataSourceListEntry(eesDataSource, "Local authority average 2023/24"),
                new DataSourceListEntry(eesDataSource, "National average 2023/24")
            ])
        ]);

        return pageResult;
    }

    public virtual async Task<IActionResult> OnGetExportAsync(string uid)
    {
        var trustSummary = await TrustService.GetTrustSummaryAsync(uid);

        if (trustSummary == null)
        {
            return new NotFoundResult();
        }

        // Sanitize the trust name to remove any illegal characters
        var sanitizedTrustName =
            string.Concat(trustSummary.Name.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));

        var fileContents = await academiesExportService.Build(uid);
        var fileName = $"{sanitizedTrustName}-{DateTimeProvider.Now:yyyy-MM-dd}.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(fileContents, contentType, fileName);
    }
}
