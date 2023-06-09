namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustSearch
{
    public Task<IEnumerable<string>> SearchAsync(string searchTerm);
}

public class TrustSearch : ITrustSearch
{
    private readonly ITrustProvider _trustProvider;

    public TrustSearch(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    public async Task<IEnumerable<string>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("No search term provided");
        }

        var trusts = await _trustProvider.GetTrustsAsync();
        return trusts.Select(t => t.GiasData?.GroupName ?? string.Empty)
            .Where(s => s.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
