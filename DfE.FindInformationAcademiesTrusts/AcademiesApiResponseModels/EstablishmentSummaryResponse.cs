using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class EstablishmentSummaryResponse
{
    public string? Urn { get; set; }
    public string? Name { get; set; }
    public string? Ukprn { get; set; }
}
