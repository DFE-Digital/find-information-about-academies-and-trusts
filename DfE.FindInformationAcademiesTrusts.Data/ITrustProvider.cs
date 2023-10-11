namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustProvider
{
    public Task<Trust?> GetTrustByUkprnAsync(string groupUid);
}
