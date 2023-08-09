using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class ApiSingleResponseV3<TResponse> where TResponse : class
{
    public TResponse? Data { get; set; }
}
