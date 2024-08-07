using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class FreeSchoolMealsModel : TrustsAreaModel, IAcademiesAreaModel
{
    private readonly IFreeSchoolMealsAverageProvider _freeSchoolMealsProvider;
    public Trust Trust { get; set; } = default!;

    public FreeSchoolMealsModel(ITrustProvider trustProvider,
        IFreeSchoolMealsAverageProvider freeSchoolMealsAverageProvider, IDataSourceService dataSourceService,
        ILogger<FreeSchoolMealsModel> logger, ITrustService trustService) :
        base(trustProvider, dataSourceService, trustService, logger, "Academies in this trust")
    {
        PageTitle = "Academies free school meals";
        _freeSchoolMealsProvider = freeSchoolMealsAverageProvider;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Trust = (await TrustProvider.GetTrustByUidAsync(Uid))!;

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

    public string TabName => "Free school meals";

    public double GetLaAverageFreeSchoolMeals(Academy academy)
    {
        return _freeSchoolMealsProvider.GetLaAverage(academy);
    }

    public double GetNationalAverageFreeSchoolMeals(Academy academy)
    {
        return _freeSchoolMealsProvider.GetNationalAverage(academy);
    }
}
