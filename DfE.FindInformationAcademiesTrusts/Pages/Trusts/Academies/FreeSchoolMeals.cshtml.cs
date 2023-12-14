using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class FreeSchoolMealsModel : TrustsAreaModel, IAcademiesAreaModel
{
    private readonly IFreeSchoolMealsAverageProvider _freeSchoolMealsProvider;

    public FreeSchoolMealsModel(ITrustProvider trustProvider,
        IFreeSchoolMealsAverageProvider freeSchoolMealsAverageProvider) :
        base(trustProvider, "Academies in this trust")
    {
        PageTitle = "Academies free school meals";
        _freeSchoolMealsProvider = freeSchoolMealsAverageProvider;
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
