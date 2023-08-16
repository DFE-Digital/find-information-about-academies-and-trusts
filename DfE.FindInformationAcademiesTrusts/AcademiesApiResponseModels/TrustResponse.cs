using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class TrustResponse
{
    public IfdDataResponse? IfdData { get; set; }
    public IfdDataResponse? TrustData { get; set; }
    public GiasDataResponse? GiasData { get; set; }
    public List<EstablishmentResponse>? Establishments { get; set; }
}
