using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class FreeSchoolMealsModel(
    IDataSourceService dataSourceService,
    ILogger<FreeSchoolMealsModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider)
    : AcademiesPageModel(dataSourceService, trustService, exportService, logger, dateTimeProvider)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { TabName = "Free school meals" };

    public AcademyFreeSchoolMealsServiceModel[] Academies { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await academyService.GetAcademiesInTrustFreeSchoolMealsAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            ["Pupils eligible for free school meals"]));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.ExploreEducationStatistics),
            ["Local authority average 2023/24", "National average 2023/24"]));

        return pageResult;
    }
}
