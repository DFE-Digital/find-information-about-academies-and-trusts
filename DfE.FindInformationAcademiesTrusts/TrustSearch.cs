namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustSearch
{
    public Task<IEnumerable<Trust>> SearchAsync(string searchTerm);
}

public class TrustSearch : ITrustSearch
{
    private readonly ITrustProvider _trustProvider;

    public TrustSearch(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    public async Task<IEnumerable<Trust>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("No search term provided");
        }

        var trusts = await _trustProvider.GetTrustsAsync();
        return trusts.Where(t => t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
