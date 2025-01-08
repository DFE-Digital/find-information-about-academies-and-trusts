using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Current;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;

public abstract class PipelineAcademiesAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IExportService exportService,
    ILogger<CurrentAcademiesAreaModel> logger,
    IDateTimeProvider dateTimeProvider
) : AcademiesAreaModel(
    dataSourceService,
    trustService,
    exportService,
    logger,
    dateTimeProvider
)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = "Pipeline" };

    public AcademyPipelineServiceModel[] Academies { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TabList =
        [
            new TrustSubNavigationLinkModel("Pre advisory board", "./PreAdvisoryBoard", Uid, "Pipeline",
                this is PreAdvisoryBoardModel),
            new TrustSubNavigationLinkModel("Post advisory board", "./PostAdvisoryBoard", Uid, "Pipeline",
                this is PostAdvisoryBoardModel),
            new TrustSubNavigationLinkModel("Free schools", "./FreeSchools", Uid, "Pipeline",
                this is FreeSchoolsModel)
        ];

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

    public static string AgeRangeSortValue(AcademyPipelineServiceModel academy)
    {
        return academy.AgeRange is not null
            ? $"{academy.AgeRange.Minimum:D2}{academy.AgeRange.Maximum:D2}"
            : string.Empty;
    }
}
