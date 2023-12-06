namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded;

public class FreeSchoolMealsAverageProvider : IFreeSchoolMealsAverageProvider
{
    private readonly Dictionary<int, FreeSchoolMealsAverage> _fsmAverages2022To23 =
        new();

    private const int NationalKey = -1;

    public FreeSchoolMealsAverageProvider()
    {
        PopulateLocalAuthorities();
        AddPercentagesByPhaseType();
    }

    private void PopulateLocalAuthorities()
    {
        _fsmAverages2022To23.Add(334, new FreeSchoolMealsAverage(334, "Solihull", "E08000029"));
    }

    private void AddPercentagesByPhaseType()
    {
        _fsmAverages2022To23[334].Add("StateFundedApSchool", 63.52941176);
    }

    public double GetLaAverage(Academy academy)
    {
        var key = PhaseTypeGroup.StateFundedApSchool;
        return _fsmAverages2022To23[334].PercentOfPupilsByPhase[key];
    }

    public double GetNationalAverage(Academy academy)
    {
        var key = PhaseTypeGroup.StateFundedApSchool;
        return _fsmAverages2022To23[NationalKey].PercentOfPupilsByPhase[key];
    }
}
