using DfE.FindInformationAcademiesTrusts.Data;

namespace test_harness;

internal class StubTrustSearch : ITrustSearch
{
    private const int PageSize = 20;

    private static readonly TrustSearchEntry[] TrustSearchEntries =
        Enumerable.Range(1000, 5000).Select(DataMakerator.CreateInstanceOfTypeFromId<TrustSearchEntry>).ToArray();

    private TrustSearchEntry[] DoSearch(string? searchTerm)
    {
        return searchTerm is null
            ? []
            : TrustSearchEntries.Where(e =>
                    e.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    e.GroupId.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
    }

    public Task<IPaginatedList<TrustSearchEntry>> SearchAsync(string? searchTerm, int page = 1)
    {
        var trustSearchEntries = DoSearch(searchTerm);

        return Task.FromResult<IPaginatedList<TrustSearchEntry>>(
            new PaginatedList<TrustSearchEntry>(
                trustSearchEntries.Skip((page - 1) * PageSize).Take(PageSize),
                trustSearchEntries.Length,
                page,
                PageSize));
    }

    public Task<TrustSearchEntry[]> SearchAutocompleteAsync(string? searchTerm)
    {
        return Task.FromResult(DoSearch(searchTerm).Take(5).ToArray());
    }
}
