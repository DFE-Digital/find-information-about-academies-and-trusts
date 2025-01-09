﻿using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Current;

public abstract class CurrentAcademiesAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    ILogger<CurrentAcademiesAreaModel> logger,
    IDateTimeProvider dateTimeProvider
) : AcademiesAreaModel(
    dataSourceService,
    trustService,
    academyService,
    exportService,
    logger,
    dateTimeProvider
)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { PageName = ViewConstants.AcademiesPageName };

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);
        var eesDataSource = await DataSourceService.GetAsync(Source.ExploreEducationStatistics);

        TabList =
        [
            new TrustSubNavigationLinkModel("Details", "./Details", Uid, "Current",
                this is CurrentAcademiesDetailsModel),
            new TrustSubNavigationLinkModel("Pupil numbers", "./PupilNumbers", Uid, "Current",
                this is PupilNumbersModel),
            new TrustSubNavigationLinkModel("Free school meals", "./FreeSchoolMeals", Uid, "Current",
                this is FreeSchoolMealsModel)
        ];

        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry(ViewConstants.AcademiesDetailsPageName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesPupilNumbersPageName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.AcademiesFreeSchoolMealsPageName, [
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
