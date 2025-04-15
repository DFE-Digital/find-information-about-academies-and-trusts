using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;

public class FreeSchoolsModel(
    IDataSourceService dataSourceService,
    ILogger<FreeSchoolsModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IPipelineAcademiesExportService pipelineAcademiesExportService,
    IDateTimeProvider dateTimeProvider)
    : PipelineAcademiesAreaModel(dataSourceService, trustService, academyService, pipelineAcademiesExportService, logger,
        dateTimeProvider)
{
    public const string TabName = "Free schools";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { TabName = TabName };

    public AcademyPipelineServiceModel[] PipelineFreeSchools { get; set; } = [];

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        PipelineFreeSchools = await AcademyService.GetAcademiesPipelineFreeSchoolsAsync(TrustReferenceNumber);

        return pageResult;
    }
}
