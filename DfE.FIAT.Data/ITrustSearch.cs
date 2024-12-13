namespace DfE.FIAT.Data;

public interface ITrustSearch
{
    Task<IPaginatedList<TrustSearchEntry>> SearchAsync(string? searchTerm, int page = 1);
    Task<TrustSearchEntry[]> SearchAutocompleteAsync(string? searchTerm);
}
