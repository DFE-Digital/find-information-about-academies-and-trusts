using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories;

public interface ITrustRepository
{
    Task<TrustSummary?> GetTrustSummaryAsync(string uid);
    Task<TrustDetails> GetTrustDetailsAsync(string uid);
}
