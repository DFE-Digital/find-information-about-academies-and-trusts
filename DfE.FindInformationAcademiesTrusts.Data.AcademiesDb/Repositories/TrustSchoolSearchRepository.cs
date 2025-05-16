using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories
{
    public class TrustSchoolSearchRepository(IAcademiesDbContext academiesDbContext) : ITrustSchoolSearchRepository
    {
        public async Task<(SearchResult[] Results, SearchResultCount NumberOfResults)> GetSearchResultsAsync(string? text, int pageSize, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return ([], new SearchResultCount(0, 0, 0));
            }

            var searchQuery = BuildSearchQuery(text);

            var totalCount = await searchQuery.CountAsync();
            var numberOfTrusts = await searchQuery.CountAsync(x => x.IsTrust);
            var numberOfSchools = await searchQuery.CountAsync(x => !x.IsTrust);

            var results = await searchQuery.OrderBy(g => g.Name)
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

            var searchQuery = BuildSearchQuery(text);

           var results = await searchQuery.OrderBy(g => g.Name)
                .Take(5)
                .ToArrayAsync();

            return results;
        }

        private IQueryable<SearchResult> BuildSearchQuery(string text)
        {
            return CreateTrustSearchQuery(text).Union(CreateSchoolSearchQuery(text));
        }

        private IQueryable<SearchResult> CreateTrustSearchQuery(string searchTerm)
        {

            var query = academiesDbContext.Groups.Trusts()
                .Where(g =>
                        g.GroupId!.Contains(searchTerm) // GroupId cannot be null for a trust
                        || g.GroupName!.Contains(searchTerm) // Enforced by global EF query filter
                )
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

        private IQueryable<SearchResult> CreateSchoolSearchQuery(string searchTerm)
        {
            var query = academiesDbContext.GiasEstablishments
                .Where(x => 
                    x.EstablishmentName!.Contains(searchTerm)
                    || x.Urn.ToString().Contains(searchTerm))
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
}
