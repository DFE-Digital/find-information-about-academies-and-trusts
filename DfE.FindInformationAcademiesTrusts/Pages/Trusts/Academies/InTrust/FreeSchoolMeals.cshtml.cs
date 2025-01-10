using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;

public class FreeSchoolMealsModel(
    IDataSourceService dataSourceService,
    ILogger<FreeSchoolMealsModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider)
    : AcademiesInTrustAreaModel(dataSourceService, trustService, academyService, exportService, logger,
        dateTimeProvider)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { TabName = ViewConstants.AcademiesInTrustFreeSchoolMealsPageName };

    public AcademyFreeSchoolMealsServiceModel[] Academies { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustFreeSchoolMealsAsync(Uid);

        return pageResult;
    }
}
