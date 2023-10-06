namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustSearch : ITrustSearch
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustHelper _trustHelper;

    public TrustSearch(IAcademiesDbContext academiesDbContext, ITrustHelper trustHelper)
    {
        _trustHelper = trustHelper;
        _academiesDbContext = academiesDbContext;
    }

    public Task<IEnumerable<TrustSearchEntry>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Task.FromResult(
                Array.Empty<TrustSearchEntry>().AsEnumerable()
            );
        }

        var trustSearchEntries = _academiesDbContext.Groups
            .Where(g => g.GroupName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase))
            .Select(g => new TrustSearchEntry(g.GroupName, _trustHelper.BuildAddressString(g), g.GroupId, 0))
            .AsEnumerable();


        return Task.FromResult(
            trustSearchEntries
        );
    }
}
