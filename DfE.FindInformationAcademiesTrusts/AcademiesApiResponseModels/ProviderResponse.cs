using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class ProviderResponse
{
    public int? Urn { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Group { get; set; }
    public int? Ukprn { get; set; }
}
