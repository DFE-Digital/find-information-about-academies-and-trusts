namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustProvider
{
    public Task<Trust?> GetTrustByGroupUidAsync(string groupUid);
}
