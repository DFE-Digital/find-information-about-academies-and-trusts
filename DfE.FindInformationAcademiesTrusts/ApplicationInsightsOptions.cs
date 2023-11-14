namespace DfE.FindInformationAcademiesTrusts;

public class ApplicationInsightsOptions
{
    public const string ConfigurationSection = "ApplicationInsights";
    
    public string? APPLICATIONINSIGHTS_CONNECTION_STRING { get; init; }
}
