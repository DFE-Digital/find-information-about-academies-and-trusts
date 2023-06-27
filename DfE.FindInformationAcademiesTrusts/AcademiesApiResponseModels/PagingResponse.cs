using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class PagingResponse
{
    public int? Page { get; set; }
    public int? RecordCount { get; set; }
    public string? NextPageUrl { get; set; }
}
