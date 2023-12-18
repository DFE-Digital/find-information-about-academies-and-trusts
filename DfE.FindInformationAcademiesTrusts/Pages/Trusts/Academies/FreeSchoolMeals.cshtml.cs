using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class FreeSchoolMealsModel : TrustsAreaModel, IAcademiesAreaModel
{
    private readonly IFreeSchoolMealsAverageProvider _freeSchoolMealsProvider;

    public FreeSchoolMealsModel(ITrustProvider trustProvider,
        IFreeSchoolMealsAverageProvider freeSchoolMealsAverageProvider, IDataSourceProvider dataSourceProvider,
        ILogger<FreeSchoolMealsModel> logger) :
        base(trustProvider, dataSourceProvider, logger, "Academies in this trust")
    {
        PageTitle = "Academies free school meals";
        _freeSchoolMealsProvider = freeSchoolMealsAverageProvider;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        DataSources.Add(new DataSourceListEntry(_freeSchoolMealsProvider.GetFreeSchoolMealsUpdated(),
            new[]
            {
                "Pupils eligible for free school meals", "Local authority average 2022/23", "National average 2022/23"
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
