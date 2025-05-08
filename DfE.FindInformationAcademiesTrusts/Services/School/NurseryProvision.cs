namespace DfE.FindInformationAcademiesTrusts.Services.School;

public enum NurseryProvision
{
    HasClasses,
    NoClasses,
    NotRecorded
}

public static class NurseryProvisionExtensions
{
    public static string ToText(this NurseryProvision provision)
    {
        return provision switch
        {
            NurseryProvision.HasClasses => "Yes",
            NurseryProvision.NoClasses => "No",
            _ => "Not Recorded"
        };
    }
}
