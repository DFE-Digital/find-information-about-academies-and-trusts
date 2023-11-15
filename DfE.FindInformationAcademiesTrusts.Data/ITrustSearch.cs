namespace DfE.FindInformationAcademiesTrusts.Data;

public interface ITrustSearch
{
    public Task<IPaginatedList<TrustSearchEntry>> SearchAsync(string searchTerm, int page = 1);
}
