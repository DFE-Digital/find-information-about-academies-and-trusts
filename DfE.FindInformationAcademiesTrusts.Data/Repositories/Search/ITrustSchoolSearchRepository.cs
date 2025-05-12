namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

public interface ITrustSchoolSearchRepository
{
    Task<SearchResult[]> GetSearchResultsAsync(string text);

    Task<(SearchResult[] results, int numberOfResults)> GetSearchResultsAsync(string text, int page, int pageSize);
}
