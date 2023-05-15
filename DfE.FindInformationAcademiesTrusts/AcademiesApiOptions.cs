using System.ComponentModel.DataAnnotations;

namespace DfE.FindInformationAcademiesTrusts;

public class AcademiesApiOptions
{
    public const string ConfigurationSection = "AcademiesApi";

    [Required] [Url] public string? Endpoint { get; init; }
    [Required] public string? Key { get; init; }
}
