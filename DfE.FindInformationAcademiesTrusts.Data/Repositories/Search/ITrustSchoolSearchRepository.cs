namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

public interface ITrustSchoolSearchRepository
{
    Task<SearchResult[]> GetSearchResultsAsync(string text);
}
