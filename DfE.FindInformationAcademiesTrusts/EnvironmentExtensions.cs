namespace DfE.FindInformationAcademiesTrusts;

public static class EnvironmentExtensions
{
    private const string CIEnvironmentName = "CI";
    private const string LocalDevelopmentEnvironmentName = "LocalDevelopment";

    public static bool IsLocalDevelopment(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(LocalDevelopmentEnvironmentName);
    }

    public static bool IsCI(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(CIEnvironmentName);
    }
}
