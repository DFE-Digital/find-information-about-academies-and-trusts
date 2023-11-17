using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustSearch : ITrustSearch
{
    private readonly IAcademiesDbContext _academiesDbContext;

    [ExcludeFromCodeCoverage] // This constructor is used by the DI container and is not unit testable
    public TrustSearch(AcademiesDbContext academiesDbContext)
        : this((IAcademiesDbContext)academiesDbContext)
    {
    }

    public TrustSearch(IAcademiesDbContext academiesDbContext)
    {
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
                new TrustSearchEntry(g.GroupName!, g.BuildAddressString(), g.GroupUid!, g.GroupId!))
            .ToArrayAsync();

        return trustSearchEntries;
    }
}
