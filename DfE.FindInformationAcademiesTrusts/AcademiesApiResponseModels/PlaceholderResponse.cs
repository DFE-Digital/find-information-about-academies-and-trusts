using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class PlaceholderResponse
{
    public string? URN { get; set; }
    public string? UKPRN { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Group { get; set; }
}
