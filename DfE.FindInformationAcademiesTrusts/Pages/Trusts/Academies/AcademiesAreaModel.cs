using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public abstract class AcademiesAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    ILogger<AcademiesAreaModel> logger,
    IDateTimeProvider dateTimeProvider
) : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { PageName = "Academies" };
    internal readonly IAcademyService AcademyService = academyService;

    protected IExportService ExportService { get; } = exportService;
    public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider;

    public AcademyPipelineSummaryServiceModel PipelineSummary { get; set; } = default!;

    public List<TrustSubNavigationLinkModel> TabList { get; set; } = [];

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;


        PipelineSummary = AcademyService.GetAcademiesPipelineSummary();

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel($"In the trust ({TrustSummary.NumberOfAcademies})",
                "/Trusts/Academies/InTrust/Details", Uid,
                TrustPageMetadata.PageName!, this is AcademiesInTrustAreaModel),
            new TrustSubNavigationLinkModel($"Pipeline academies ({PipelineSummary.Total})",
                "/Trusts/Academies/Pipeline/PreAdvisoryBoard", Uid,
                TrustPageMetadata.PageName!, this is PipelineAcademiesAreaModel)
        ];

        return pageResult;
    }
}
