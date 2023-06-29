using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class NameAndCodeResponse
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}
