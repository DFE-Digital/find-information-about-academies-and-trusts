namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustProvider
{
    Task<Trust?> GetTrustByUidAsync(string uid);
}
