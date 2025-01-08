using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Current;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public abstract class AcademiesAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IExportService exportService,
    ILogger<AcademiesAreaModel> logger,
    IDateTimeProvider dateTimeProvider
) : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { PageName = "Academies" };
    protected IExportService ExportService { get; } = exportService;
    public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider;

    public List<TrustSubNavigationLinkModel> TabList { get; set; } = [];

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel("In the trust", "/Trusts/Academies/Current/Details", Uid,
                TrustPageMetadata.PageName!, this is CurrentAcademiesAreaModel),
            new TrustSubNavigationLinkModel("Pipeline academies", "/Trusts/Academies/Pipeline/PreAdvisoryBoard", Uid,
                TrustPageMetadata.PageName!, this is PipelineAcademiesAreaModel)
        ];

        return pageResult;
    }
}
