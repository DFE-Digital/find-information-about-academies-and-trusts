using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Search;

public record PagedSearchResults(IPaginatedList<SearchResultServiceModel> ResultsList, SearchResultsOverview ResultsOverview);
