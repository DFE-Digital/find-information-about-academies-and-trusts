using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;

public class PostAdvisoryBoardModel(
    IDataSourceService dataSourceService,
    ILogger<PostAdvisoryBoardModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider)
    : PipelineAcademiesAreaModel(dataSourceService, trustService, academyService, exportService, logger,
        dateTimeProvider)
{
    public const string TabName = "Post advisory board";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { TabName = TabName };

    public AcademyPipelineServiceModel[] PostAdvisoryPipelineEstablishments { get; set; } = [];

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        PostAdvisoryPipelineEstablishments =
            await AcademyService.GetAcademiesPipelinePostAdvisoryAsync(TrustReferenceNumber);

        return pageResult;
    }
}
