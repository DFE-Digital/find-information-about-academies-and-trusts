namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustSearch : ITrustSearch
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustHelper _trustHelper;

    public TrustSearch(AcademiesDbContext academiesDbContext, ITrustHelper trustHelper)
        : this((IAcademiesDbContext)academiesDbContext, trustHelper)
    {
    }

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
            .Where(g => g.GroupName != null &&
                        g.GroupName.Contains(
                            searchTerm)) //note that LINQ translates string.contains to case insensitive SQL
            .OrderBy(g => g.GroupName)
            .Take(20)
            .Select(g =>
                new TrustSearchEntry(g.GroupName, _trustHelper.BuildAddressString(g), g.GroupUid, g.GroupId))
            .AsEnumerable();


        return Task.FromResult(
            trustSearchEntries
        );
    }
}
