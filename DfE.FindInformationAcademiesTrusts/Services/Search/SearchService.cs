using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

namespace DfE.FindInformationAcademiesTrusts.Services.Search;

public interface ISearchService
{
    Task<SearchResult[]> GetSearchResultsForAutocompleteAsync(string text);
    Task<IPaginatedList<SearchResult>> GetSearchResultsForPageAsync(string? keyWords, int pageNumber);
}

public class SearchService(ITrustSchoolSearchRepository trustSchoolSearchRepository, IStringFormattingUtilities stringFormattingUtilities) : ISearchService
{
    private int PageSize = 20;

    public async Task<SearchResult[]> GetSearchResultsForAutocompleteAsync(string text)
    {
        var results = await trustSchoolSearchRepository.GetSearchResultsAsync(text);

        return results.Select(x =>
                new SearchResult(x.Name, stringFormattingUtilities.BuildAddressString(x.Street, x.Locality, x.Town, x.PostCode), x.Identifier, x.IsTrust ? ResultType.Trust : ResultType.School))
            .ToArray();
    }

    public async Task<IPaginatedList<SearchResult>> GetSearchResultsForPageAsync(string? keyWords, int pageNumber)
    {
        if (string.IsNullOrWhiteSpace(keyWords))
        {
            return PaginatedList<SearchResult>.Empty();
        }

        var searchResults = await trustSchoolSearchRepository.GetSearchResultsAsync(keyWords, pageNumber, PageSize);

        var results = searchResults.results.Select(x =>
                new SearchResult(x.Name, stringFormattingUtilities.BuildAddressString(x.Street, x.Locality, x.Town, x.PostCode), x.Identifier, x.IsTrust ? ResultType.Trust : ResultType.School))
            .ToArray();

        return new PaginatedList<SearchResult>(results, searchResults.numberOfResults, pageNumber, PageSize);
    }
}
