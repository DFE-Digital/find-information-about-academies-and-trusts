using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

namespace DfE.FindInformationAcademiesTrusts.Services.Search;

public interface ISearchService
{
    Task<SearchResult[]> GetSearchResultsForAutocompleteAsync(string text);
}

public class SearchService(ITrustSchoolSearchRepository trustSchoolSearchRepository) : ISearchService
{
    public async Task<SearchResult[]> GetSearchResultsForAutocompleteAsync(string text)
    {
        var result = await trustSchoolSearchRepository.GetSearchResultsAsync(text);

        return result.Select(x =>
                new SearchResult(x.Name, x.Address, x.Identifier, x.IsTrust ? ResultType.Trust : ResultType.School))
            .ToArray();
    }
}
