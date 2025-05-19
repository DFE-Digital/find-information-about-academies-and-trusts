using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class TrustSchoolSearchRepository(IAcademiesDbContext academiesDbContext) : ITrustSchoolSearchRepository
{
    public async Task<(SearchResult[] Results, SearchResultCount NumberOfResults)> GetSearchResultsAsync(string? text,
        int pageSize, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return ([], new SearchResultCount(0, 0, 0));
        }

        //Count is done without the select for speed efficiency
        var numberOfTrusts = await CreateTrustSearchQuery(text).CountAsync();
        var numberOfSchools = await CreateSchoolSearchQuery(text).CountAsync();
        var totalCount = numberOfTrusts + numberOfSchools;

        //If there's no results then don't go back to the db
        if (totalCount == 0)
            return ([], new SearchResultCount(0, 0, 0));

        //Now get all the results
        var results = await BuildOrderedSearchResultQuery(text)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        return (results, new SearchResultCount(totalCount, numberOfTrusts, numberOfSchools));
    }

    public async Task<SearchResult[]> GetAutoCompleteSearchResultsAsync(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        var results = await BuildOrderedSearchResultQuery(text)
            .Take(5)
            .ToArrayAsync();

        return results;
    }

    private IQueryable<GiasGroup> CreateTrustSearchQuery(string searchTerm)
    {
        return academiesDbContext.Groups.Trusts()
            .Where(g =>
                    g.GroupId!.Contains(searchTerm) // GroupId cannot be null for a trust
                    || g.GroupName!.Contains(searchTerm) // Enforced by global EF query filter
            );
    }

    private IQueryable<GiasEstablishment> CreateSchoolSearchQuery(string searchTerm)
    {
        return academiesDbContext.GiasEstablishments
            .Where(x =>
                x.EstablishmentName!.Contains(searchTerm) // Enforced by global EF query filter
                || x.Urn.ToString().Contains(searchTerm));
    }

    private IQueryable<SearchResult> BuildOrderedSearchResultQuery(string text)
    {
        return SelectTrusts(CreateTrustSearchQuery(text))
            .Union(SelectSchools(CreateSchoolSearchQuery(text)))
            //Two entities could have the same (case-insensitive) name (e.g. a single academy trust or a school with a
            //common name like "St. Mary's") so ensure that we order by another property too for consistent returns
            .OrderBy(g => g.Name)
            .ThenBy(g => g.Id);
    }

    private static IQueryable<SearchResult> SelectTrusts(IQueryable<GiasGroup> trustsBaseQuery)
    {
        var query = trustsBaseQuery
            .Select(g => new SearchResult
            {
                Street = g.GroupContactStreet,
                Locality = g.GroupContactLocality,
                Town = g.GroupContactTown,
                PostCode = g.GroupContactPostcode,
                Name = g.GroupName!,
                Id = g.GroupUid!.ToString(),
                TrustGroupId = g.GroupId!.ToString(),
                Type = g.GroupType!,
                IsTrust = true
            });
        return query;
    }

    private static IQueryable<SearchResult> SelectSchools(IQueryable<GiasEstablishment> schoolsBaseQuery)
    {
        var query = schoolsBaseQuery
            .Select(e => new SearchResult
            {
                Street = e.Street,
                Locality = e.Locality,
                Town = e.Town,
                PostCode = e.Postcode,
                TrustGroupId = null,
                Name = e.EstablishmentName!,
                Id = e.Urn.ToString(),
                Type = e.TypeOfEstablishmentName!,
                IsTrust = false
            });
        return query;
    }
}
