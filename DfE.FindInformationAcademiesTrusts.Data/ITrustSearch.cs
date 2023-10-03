namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustSearch
{
    public Task<IEnumerable<TrustSearchEntry>> SearchAsync(string searchTerm);
}
