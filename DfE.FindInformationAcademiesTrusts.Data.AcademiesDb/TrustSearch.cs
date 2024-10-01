using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public class TrustSearch : ITrustSearch
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private const int PageSize = 20;

    public TrustSearch(IAcademiesDbContext academiesDbContext)
    {
        _academiesDbContext = academiesDbContext;
    }

    public async Task<IPaginatedList<TrustSearchEntry>> SearchAsync(string searchTerm, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return PaginatedList<TrustSearchEntry>.Empty();
        }
        var lowerSearchTerm = searchTerm.ToLower();

        var query = _academiesDbContext.Groups
            .Where(g =>
                g.GroupUid != null &&
                g.GroupId != null &&
                g.GroupName != null &&
                g.GroupType != null &&
                (g.GroupType == "Multi-academy trust" || g.GroupType == "Single-academy trust") &&
                (
                    g.GroupId.ToLower().Contains(lowerSearchTerm) ||
                    g.GroupName.ToLower().Contains(lowerSearchTerm)
                )
            );


        var count = await query.CountAsync();
        var trustSearchEntries = await query
            .OrderBy(g => g.GroupName)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(g =>
                new TrustSearchEntry(g.GroupName!, g.BuildAddressString(), g.GroupUid!, g.GroupId!))
            .ToArrayAsync();

        return new PaginatedList<TrustSearchEntry>(trustSearchEntries, count, page, PageSize);
    }
}
