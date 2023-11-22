namespace DfE.FindInformationAcademiesTrusts;

public class ApplicationInsightsOptions
{
    public const string ConfigurationSection = "ApplicationInsights";

    public string? ConnectionString { get; init; }
}
