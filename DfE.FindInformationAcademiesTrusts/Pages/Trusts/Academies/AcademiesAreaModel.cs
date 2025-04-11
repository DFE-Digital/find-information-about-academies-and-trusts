using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public abstract class AcademiesAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    ILogger<AcademiesAreaModel> logger,
    IDateTimeProvider dateTimeProvider
) : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public const string PageName = "Academies";
    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { PageName = PageName };

    internal readonly IAcademyService AcademyService = academyService;
    public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider;

    public AcademyPipelineSummaryServiceModel PipelineSummary { get; set; } = default!;
    public string TrustReferenceNumber { get; set; } = default!;

    public List<TrustTabNavigationLinkModel> TabList { get; set; } = [];

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;
        TrustReferenceNumber = await TrustService.GetTrustReferenceNumberAsync(Uid);

        PipelineSummary = await AcademyService.GetAcademiesPipelineSummaryAsync(TrustReferenceNumber);
        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel($"In this trust ({TrustSummary.NumberOfAcademies})",
                "/Trusts/Academies/InTrust/Details", Uid, TrustPageMetadata.PageName!,
                this is AcademiesInTrustAreaModel),
            new TrustSubNavigationLinkModel($"Pipeline academies ({PipelineSummary.Total})",
                "/Trusts/Academies/Pipeline/PreAdvisoryBoard", Uid, TrustPageMetadata.PageName!,
                this is PipelineAcademiesAreaModel)
        ];

        return pageResult;
    }
}
