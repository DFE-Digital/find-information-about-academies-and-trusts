using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public class OfstedAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider,
    ILogger<OfstedAreaModel> logger)
    : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { PageName = ViewConstants.OfstedPageName };

    public AcademyOfstedServiceModel[] Academies { get; set; } = default!;
    private IAcademyService AcademyService { get; } = academyService;
    protected IExportService ExportService { get; } = exportService;
    public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustOfstedAsync(Uid);

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel(ViewConstants.OfstedSingleHeadlineGradesPageName, "./SingleHeadlineGrades",
                Uid, TrustPageMetadata.PageName!, this is SingleHeadlineGradesModel),
            new TrustSubNavigationLinkModel(ViewConstants.OfstedCurrentRatingsPageName, "./CurrentRatings", Uid,
                TrustPageMetadata.PageName!, this is CurrentRatingsModel),
            new TrustSubNavigationLinkModel(ViewConstants.OfstedPreviousRatingsPageName, "./PreviousRatings", Uid,
                TrustPageMetadata.PageName!, this is PreviousRatingsModel),
            new TrustSubNavigationLinkModel(ViewConstants.OfstedSafeguardingAndConcernsPageName,
                "./SafeguardingAndConcerns", Uid, TrustPageMetadata.PageName!, this is SafeguardingAndConcernsModel)
        ];

        // Add data sources
        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);
        var misDataSource = await DataSourceService.GetAsync(Source.Mis);

        var dateJoinedTrust = new DataSourceListEntry(giasDataSource, "Date joined trust");

        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry(ViewConstants.OfstedCurrentRatingsPageName, [
                    new DataSourceListEntry(misDataSource, "Current Ofsted rating"),
                    new DataSourceListEntry(misDataSource, "Date of current inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedPreviousRatingsPageName, [
                    new DataSourceListEntry(misDataSource, "Previous Ofsted rating"),
                    new DataSourceListEntry(misDataSource, "Date of previous inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedSingleHeadlineGradesPageName, [
                    dateJoinedTrust,
                    new DataSourceListEntry(misDataSource, "Date of current inspection"),
                    new DataSourceListEntry(misDataSource, "Date of previous inspection")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.OfstedSafeguardingAndConcernsPageName, [
                    new DataSourceListEntry(misDataSource, "Effective safeguarding"),
                    new DataSourceListEntry(misDataSource, "Category of concern"),
                    new DataSourceListEntry(misDataSource, "Date of current inspection")
                ]
            )
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

        var fileContents = await ExportService.ExportOfstedDataToSpreadsheetAsync(uid);
        var fileName = $"Ofsted-{sanitizedTrustName}-{DateTimeProvider.Now:yyyy-MM-dd}.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(fileContents, contentType, fileName);
    }
}
