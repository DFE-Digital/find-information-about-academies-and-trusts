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
    : TrustsAreaModel(dataSourceService, trustService, logger, "Ofsted")
{
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
            new TrustSubNavigationLinkModel("Current ratings", "./CurrentRatings", Uid, PageName,
                this is CurrentRatingsModel),
            new TrustSubNavigationLinkModel("Previous ratings", "./PreviousRatings", Uid,
                PageName, this is PreviousRatingsModel),
            new TrustSubNavigationLinkModel("Important dates", "./ImportantDates", Uid, PageName,
                this is ImportantDatesModel),
            new TrustSubNavigationLinkModel("Safeguarding and concerns", "./SafeguardingAndConcerns", Uid,
                PageName, this is SafeguardingAndConcernsModel)
        ];

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            ["Date joined trust"]));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Mis),
        [
            "Current Ofsted rating",
            "Date of last inspection",
            "Previous Ofsted rating",
            "Date of previous inspection"
        ]));

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
