namespace DfE.FindInformationAcademiesTrusts.Options;

public class TestOverrideOptions
{
    public const string ConfigurationSection = "TestOverride";

    public string? PlaywrightTestSecret { get; init; }
}
