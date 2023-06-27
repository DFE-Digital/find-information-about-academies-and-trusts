using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class ViewAcademyConversionResponse
{
    public string? ViabilityIssue { get; set; }
    public string? PFI { get; set; }
    public string? PAN { get; set; }
    public string? Deficit { get; set; }
}
