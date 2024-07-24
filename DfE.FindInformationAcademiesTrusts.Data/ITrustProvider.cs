using DfE.FindInformationAcademiesTrusts.Data.Dto;

namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustProvider
{
    Task<Trust?> GetTrustByUidAsync(string uid);
    Task<TrustSummaryDto?> GetTrustSummaryAsync(string uid);
    Task<TrustDetailsDto> GetTrustDetailsAsync(string uid);
}
