using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories
{
    public class TrustSchoolSearchRepository(IAcademiesDbContext academiesDbContext) : ITrustSchoolSearchRepository
    {
        public async Task<(SearchResult[] results, int numberOfResults)> GetSearchResultsAsync(string text, int page, int pageSize)
        {
            var searchQuery = CreateTrustSearchQuery(text).Union(CreateSchoolSearchQuery(text));

            var count = await searchQuery.CountAsync();

            var results = await searchQuery.OrderBy(g => g.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            return (results, count);
        }

        public async Task<SearchResult[]> GetSearchResultsAsync(string text)
        {
            var searchQuery = CreateTrustSearchQuery(text).Union(CreateSchoolSearchQuery(text));

           var results = await searchQuery.OrderBy(g => g.Name)
                .Take(5)
                .ToArrayAsync();

            return results;
        }

        private IQueryable<SearchResult> CreateTrustSearchQuery(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();

            var query = academiesDbContext.Groups.Trusts()
                .Where(g =>
                        g.GroupId!.ToLower().Contains(lowerSearchTerm) // GroupId cannot be null for a trust
                        || g.GroupName!.ToLower().Contains(lowerSearchTerm) // Enforced by global EF query filter
                )
                .Select(g => new SearchResult
                {
                    Street = g.GroupContactStreet,
                    Locality = g.GroupContactLocality,
                    Town = g.GroupContactTown,
                    PostCode = g.GroupContactPostcode,
                    Name = g.GroupName!,
                    Identifier = g.GroupUid!.ToString(),
                    IsTrust = true
                });
            return query;
        }

        private IQueryable<SearchResult> CreateSchoolSearchQuery(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();

            var query = academiesDbContext.GiasEstablishments
                .Where(x => x.EstablishmentName!.ToLower().Contains(lowerSearchTerm))
                .Select(e => new SearchResult
                {
                    Street = e.Street,
                    Locality = e.Locality,
                    Town = e.Town,
                    PostCode = e.Postcode,
                    Name = e.EstablishmentName!,
                    Identifier = e.Urn.ToString(),
                    IsTrust = false
                });
            return query;

        }
    }
}
