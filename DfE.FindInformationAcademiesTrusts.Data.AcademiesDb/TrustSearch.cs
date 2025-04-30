using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

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
                    g.GroupName!, //Enforced by global EF filter
                    stringFormattingUtilities.BuildAddressString(
                        g.GroupContactStreet,
                        g.GroupContactLocality,
                        g.GroupContactTown,
                        g.GroupContactPostcode),
                    g.GroupUid!, //Enforced by global EF filter
                    g.GroupId! // GroupId cannot be null for a trust
                ))
            .ToArrayAsync();

        return new PaginatedList<TrustSearchEntry>(trustSearchEntries, count, page, PageSize);
    }

    private IQueryable<GiasGroup> CreateSearchQuery(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();

        var query = academiesDbContext.Groups.Trusts()
            .Where(g =>
                    g.GroupId!.ToLower().Contains(lowerSearchTerm) // GroupId cannot be null for a trust
                    || g.GroupName!.ToLower().Contains(lowerSearchTerm) // Enforced by global EF query filter
            );
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
                        g.GroupName!, //Enforced by global EF filter
                        stringFormattingUtilities.BuildAddressString(
                            g.GroupContactStreet,
                            g.GroupContactLocality,
                            g.GroupContactTown,
                            g.GroupContactPostcode),
                        g.GroupUid!, //Enforced by global EF filter
                        g.GroupId! // GroupId cannot be null for a trust
                    ))
                .ToArrayAsync();

        return trustSearchEntries;
    }
}
