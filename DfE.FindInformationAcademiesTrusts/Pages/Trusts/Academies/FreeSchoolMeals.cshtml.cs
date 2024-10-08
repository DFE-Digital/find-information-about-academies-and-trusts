using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class FreeSchoolMealsModel : AcademiesPageModel
{
    public IAcademyService AcademyService { get; }
    public AcademyFreeSchoolMealsServiceModel[] Academies { get; set; } = default!;

    public FreeSchoolMealsModel(ITrustProvider trustProvider, IDataSourceService dataSourceService,
        ILogger<FreeSchoolMealsModel> logger, ITrustService trustService, IAcademyService academyService,
        IExportService exportService, IDateTimeProvider dateTimeProvider) :
        base(trustProvider, dataSourceService, trustService, academyService, exportService, logger, dateTimeProvider)
    {
        PageTitle = "Academies free school meals";
        TabName = "Free school meals";
        AcademyService = academyService;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustFreeSchoolMealsAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new[]
            {
                "Pupils eligible for free school meals"
            }));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.ExploreEducationStatistics),
            new[]
            {
                "Local authority average 2023/24", "National average 2023/24"
            }));

        return pageResult;
    }
}
