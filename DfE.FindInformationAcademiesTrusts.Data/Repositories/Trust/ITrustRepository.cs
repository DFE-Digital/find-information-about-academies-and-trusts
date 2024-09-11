namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

public interface ITrustRepository
{
    Task<TrustSummary?> GetTrustSummaryAsync(string uid);
    Task<TrustDetails> GetTrustDetailsAsync(string uid);
    Task<TrustGovernance> GetTrustGovernanceAsync(string uid);
    Task<TrustContacts> GetTrustContactsAsync(string uid);
}
