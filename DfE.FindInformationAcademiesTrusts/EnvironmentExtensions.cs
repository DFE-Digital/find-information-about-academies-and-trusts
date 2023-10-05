namespace DfE.FindInformationAcademiesTrusts;

public static class EnvironmentExtensions
{
    private const string ContinuousIntegrationEnvironmentName = "CI";
    private const string LocalDevelopmentEnvironmentName = "LocalDevelopment";
    private const string TestEnvironmentName = "Test";

    public static bool IsLocalDevelopment(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(LocalDevelopmentEnvironmentName);
    }

    public static bool IsContinuousIntegration(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(ContinuousIntegrationEnvironmentName);
    }

    public static bool IsTest(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(TestEnvironmentName);
    }
}
