namespace DfE.FindInformationAcademiesTrusts;

public class AcademiesApiOptions
{
    public const string ConfigurationSection = "AcademiesApi";

    public string? Endpoint { get; set; }
    public string? Key { get; set; }
}
