namespace DfE.FindInformationAcademiesTrusts;

public static class EnvironmentExtensions
{
    private const string LocalDevelopmentEnvironmentName = "Local Development";

    public static bool IsLocalDevelopment(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(LocalDevelopmentEnvironmentName);
    }
}
