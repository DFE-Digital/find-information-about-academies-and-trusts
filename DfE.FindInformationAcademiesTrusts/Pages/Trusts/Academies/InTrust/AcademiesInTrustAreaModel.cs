using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;

public abstract class AcademiesInTrustAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    ILogger<AcademiesInTrustAreaModel> logger,
    IDateTimeProvider dateTimeProvider,
    IFeatureManager featureManager
) : AcademiesAreaModel(
    dataSourceService,
    trustService,
    academyService,
    exportService,
    logger,
    dateTimeProvider,
    featureManager
)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with
        {
            PageName = ViewConstants.AcademiesPageName, SubPageName = ViewConstants.AcademiesInTrustSubNavName
        };

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);
        var eesDataSource = await DataSourceService.GetAsync(Source.ExploreEducationStatistics);

        TabList =
        [
            new TrustTabNavigationLinkModel("Details", "./Details", Uid, "In the trust",
                this is AcademiesInTrustDetailsModel),
            new TrustTabNavigationLinkModel("Pupil numbers", "./PupilNumbers", Uid, "In the trust",
                this is PupilNumbersModel),
            new TrustTabNavigationLinkModel("Free school meals", "./FreeSchoolMeals", Uid, "In the trust",
                this is FreeSchoolMealsModel)
        ];

        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry(ViewConstants.AcademiesInTrustDetailsPageName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesInTrustPupilNumbersPageName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesInTrustFreeSchoolMealsPageName, [
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

        var fileContents = await ExportService.ExportAcademiesToSpreadsheetAsync(uid);
        var fileName = $"{sanitizedTrustName}-{DateTimeProvider.Now:yyyy-MM-dd}.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(fileContents, contentType, fileName);
    }
}
