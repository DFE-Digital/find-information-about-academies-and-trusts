using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;

public class PreAdvisoryBoardModel(
    IDataSourceService dataSourceService,
    ILogger<PreAdvisoryBoardModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider)
    : PipelineAcademiesAreaModel(dataSourceService, trustService, academyService, exportService, logger,
        dateTimeProvider)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { TabName = "Pre advisory board" };

    public AcademyPipelineServiceModel[] PreAdvisoryPipelineEstablishments { get; set; } = [];

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        PreAdvisoryPipelineEstablishments =
            await AcademyService.GetAcademiesPipelinePreAdvisoryAsync(TrustReferenceNumber);

        return pageResult;
    }
}
