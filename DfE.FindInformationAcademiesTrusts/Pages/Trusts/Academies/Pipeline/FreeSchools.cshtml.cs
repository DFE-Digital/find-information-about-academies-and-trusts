using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Current;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;

public class FreeSchoolsModel(
    IDataSourceService dataSourceService,
    ILogger<PupilNumbersModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider)
    : PipelineAcademiesAreaModel(dataSourceService, trustService, academyService, exportService, logger,
        dateTimeProvider)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { TabName = "Free schools" };

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = AcademyService.GetAcademiesPipelinePreAdvisory();

        return pageResult;
    }
}
