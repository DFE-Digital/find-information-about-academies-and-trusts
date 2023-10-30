using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustSearch : ITrustSearch
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly ITrustHelper _trustHelper;

    [ExcludeFromCodeCoverage] // This constructor is used by the DI container and is not unit testable
    public TrustSearch(AcademiesDbContext academiesDbContext, ITrustHelper trustHelper)
        : this((IAcademiesDbContext)academiesDbContext, trustHelper)
    {
    }

    public TrustSearch(IAcademiesDbContext academiesDbContext, ITrustHelper trustHelper)
    {
        _trustHelper = trustHelper;
        _academiesDbContext = academiesDbContext;
    }

    public async Task<TrustSearchEntry[]> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Array.Empty<TrustSearchEntry>();
        }

        var trustSearchEntries = await _academiesDbContext.Groups
            .Where(g =>
                g.GroupUid != null &&
                g.GroupId != null &&
                g.GroupName != null &&
                g.GroupName.Contains(searchTerm) &&
                g.GroupType != null &&
                (g.GroupType == "Multi-academy trust" ||
                 g.GroupType == "Single-academy trust")
            ) //note that LINQ translates string.contains to case insensitive SQL
            .OrderBy(g => g.GroupName)
            .Take(20)
            .Select(g =>
                new TrustSearchEntry(g.GroupName!, _trustHelper.BuildAddressString(g), g.GroupUid!, g.GroupId!))
            .ToArrayAsync();

        return trustSearchEntries;
    }
}