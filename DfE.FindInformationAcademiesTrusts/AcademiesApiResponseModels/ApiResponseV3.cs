using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class ApiResponseV3<TResponse> where TResponse : class
{
    public IEnumerable<TResponse>? Data { get; set; }
    public PagingResponse? Paging { get; set; }
}

[ExcludeFromCodeCoverage] // API response model POCO
public class ApiTrustResponseV3
{
    public TrustResponse? Data { get; set; }
}
