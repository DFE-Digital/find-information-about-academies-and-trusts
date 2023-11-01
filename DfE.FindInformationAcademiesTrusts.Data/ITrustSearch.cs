namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustSearch
{
    public Task<TrustSearchEntry[]> SearchAsync(string searchTerm);
}
