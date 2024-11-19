namespace DfE.FIAT.Data.Hardcoded;

public class FreeSchoolMealsAverage
{
    public int OldLaCode { get; }
    public string LaName { get; }
    public string? NewLaCode { get; }

    public Dictionary<ExploreEducationStatisticsPhaseType, double> PercentOfPupilsByPhase { get; } = new();

    public FreeSchoolMealsAverage(int oldLaCode, string laName, string? newLaCode = null)
    {
        OldLaCode = oldLaCode;
        LaName = laName;
        NewLaCode = newLaCode;
    }
}
