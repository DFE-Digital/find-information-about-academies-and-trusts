using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories
{
    public class TrustSchoolSearchRepository(IAcademiesDbContext academiesDbContext) : ITrustSchoolSearchRepository
    {
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
                .Select(r => new SearchResult
                {
                    Address = r.GroupContactStreet!,
                    Name = r.GroupName!,
                    Identifier = r.GroupUid!.ToString(),
                    IsTrust = true
                });
            return query;
        }

        private IQueryable<SearchResult> CreateSchoolSearchQuery(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();

            var query = academiesDbContext.GiasEstablishments
                .Where(x => x.EstablishmentName!.ToLower().Contains(lowerSearchTerm))
                .Select(r => new SearchResult
                {
                    Address = r.Postcode!,
                    Name = r.EstablishmentName!,
                    Identifier = r.Urn.ToString(),
                    IsTrust = false
                });
            return query;

        }
    }
}
