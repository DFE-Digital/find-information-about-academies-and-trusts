namespace DfE.FindInformationAcademiesTrusts;

public static class EnvironmentExtensions
{
    private const string ContinuousIntegrationEnvironmentName = "CI";
    private const string LocalDevelopmentEnvironmentName = "LocalDevelopment";

    public static bool IsLocalDevelopment(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(LocalDevelopmentEnvironmentName);
    }

    public static bool IsContinuousIntegration(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(ContinuousIntegrationEnvironmentName);
    }
}
