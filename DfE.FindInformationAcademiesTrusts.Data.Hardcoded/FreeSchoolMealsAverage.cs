namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded;

public class FreeSchoolMealsAverage
{
    private int OldLaCode { get; }
    private string LaName { get; }
    private string NewLaCode { get; }

    public Dictionary<ExploreEducationStatisticsPhaseType, double> PercentOfPupilsByPhase { get; } = new();

    public FreeSchoolMealsAverage(int oldLaCode, string laName, string newLaCode)
    {
        OldLaCode = oldLaCode;
        LaName = laName;
        NewLaCode = newLaCode;
    }

    public void Add(string phaseTypeGroupKey, double percentOfPupils)
    {
        var key = Enum.Parse<ExploreEducationStatisticsPhaseType>(phaseTypeGroupKey);
        PercentOfPupilsByPhase.Add(key, percentOfPupils);
    }
}
