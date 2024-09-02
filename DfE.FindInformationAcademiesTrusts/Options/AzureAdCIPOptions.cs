namespace DfE.FindInformationAcademiesTrusts.Options;

public class AzureAdCIPOptions
{
    public const string ConfigurationSection = "AzureAdCIP";

    public string? ConnectionString { get; init; }
    public string? Instance { get; init; }
    public string? Domain { get; init; }
    public string? TenantId { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
}
