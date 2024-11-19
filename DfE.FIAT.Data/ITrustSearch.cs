namespace DfE.FIAT.Data;

public interface ITrustSearch
{
    public Task<IPaginatedList<TrustSearchEntry>> SearchAsync(string searchTerm, int page = 1);
}
