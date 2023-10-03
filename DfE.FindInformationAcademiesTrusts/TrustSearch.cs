using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts;

public class TrustSearch : ITrustSearch
{
    private readonly ITrustProvider _trustProvider;

    public TrustSearch(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    public async Task<IEnumerable<TrustSearchEntry>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("No search term provided");
        }

        var trusts = await _trustProvider.GetTrustsAsync();
        return trusts.Where(t => t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
