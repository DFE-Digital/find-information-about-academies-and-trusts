namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustProvider
{
    public Task<Trust?> GetTrustByUidAsync(string uid);
}
