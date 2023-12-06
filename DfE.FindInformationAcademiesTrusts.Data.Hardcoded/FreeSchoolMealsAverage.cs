using System.ComponentModel;

namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded;

public enum PhaseTypeGroup
{
    [Description("State-funded AP school")]
    StateFundedApSchool,
    [Description("State-funded primary")] StateFundedPrimary,

    [Description("State-funded secondary")]
    StateFundedSecondary,

    [Description("State-funded special school")]
    StateFundedSpecialSchool
}

public class FreeSchoolMealsAverage
{
    private int OldLaCode { get; }
    private string LaName { get; }
    private string NewLaCode { get; }

    public Dictionary<PhaseTypeGroup, double> PercentOfPupilsByPhase { get; } = new();

    public FreeSchoolMealsAverage(int oldLaCode, string laName, string newLaCode)
    {
        OldLaCode = oldLaCode;
        LaName = laName;
        NewLaCode = newLaCode;
    }

    public void Add(string phaseTypeGroupKey, double percentOfPupils)
    {
        var key = Enum.Parse<PhaseTypeGroup>(phaseTypeGroupKey);
        PercentOfPupilsByPhase.Add(key, percentOfPupils);
    }
}
