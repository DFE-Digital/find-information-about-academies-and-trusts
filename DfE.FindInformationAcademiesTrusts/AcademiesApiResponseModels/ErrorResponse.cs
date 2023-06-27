using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class ErrorResponse
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
}
