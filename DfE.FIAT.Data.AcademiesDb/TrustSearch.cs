using DfE.FIAT.Data.AcademiesDb.Contexts;
using DfE.FIAT.Data.AcademiesDb.Models.Gias;
using Microsoft.EntityFrameworkCore;

namespace DfE.FIAT.Data.AcademiesDb;

public class TrustSearch(IAcademiesDbContext academiesDbContext, IStringFormattingUtilities stringFormattingUtilities)
    : ITrustSearch
{
    private const int PageSize = 20;

    public async Task<IPaginatedList<TrustSearchEntry>> SearchAsync(string? searchTerm, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return PaginatedList<TrustSearchEntry>.Empty();
        }

        var query = CreateSearchQuery(searchTerm);

        var count = await query.CountAsync();

        var trustSearchEntries = await query
            .OrderBy(g => g.GroupName)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(g =>
                new TrustSearchEntry(
                    g.GroupName!, //Enforced by EF filter
                    stringFormattingUtilities.BuildAddressString(
                        g.GroupContactStreet,
                        g.GroupContactLocality,
                        g.GroupContactTown,
                        g.GroupContactPostcode),
                    g.GroupUid!, //Enforced by EF filter
                    g.GroupId! //Enforced by EF filter
                ))
            .ToArrayAsync();

        return new PaginatedList<TrustSearchEntry>(trustSearchEntries, count, page, PageSize);
    }

    private IQueryable<GiasGroup> CreateSearchQuery(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();

        var query = academiesDbContext.Groups
            .Where(g =>
                g.GroupId!.ToLower().Contains(lowerSearchTerm)
                || g.GroupName!.ToLower().Contains(lowerSearchTerm)
            ); // GroupId and GroupName cannot be null because they are in EF query filters
        return query;
    }

    public async Task<TrustSearchEntry[]> SearchAutocompleteAsync(string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return [];
        }

        var trustSearchEntries =
            await CreateSearchQuery(searchTerm)
                .OrderBy(g => g.GroupName)
                .Take(5)
                .Select(g =>
                    new TrustSearchEntry(
                        g.GroupName!, //Enforced by EF filter
                        stringFormattingUtilities.BuildAddressString(
                            g.GroupContactStreet,
                            g.GroupContactLocality,
                            g.GroupContactTown,
                            g.GroupContactPostcode),
                        g.GroupUid!, //Enforced by EF filter
                        g.GroupId! //Enforced by EF filter
                    ))
                .ToArrayAsync();

        return trustSearchEntries;
    }
}
