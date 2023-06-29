using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class ViewAcademyConversionResponse
{
    public string? ViabilityIssue { get; set; }
    public string? Pfi { get; set; }
    public string? Pan { get; set; }
    public string? Deficit { get; set; }
}
