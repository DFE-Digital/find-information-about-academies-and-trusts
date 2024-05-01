namespace DfE.FindInformationAcademiesTrusts.Options;

public class TestOverrideOptions
{
    public const string ConfigurationSection = "TestOverride";

    public string? CypressTestSecret { get; init; }
}
