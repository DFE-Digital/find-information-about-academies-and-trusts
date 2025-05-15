using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Search;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : ContentPageModel, IPageSearchFormModel, IPaginationModel
{
    public record AutocompleteEntry(string Id, string Address, string Name, ResultType ResultType);

    private readonly ITrustService _trustService;
    private readonly ISearchService _searchService;
    private readonly ISchoolService _schoolService;

    public string PageName => "Search";
    public IPageStatus PageStatus => SearchResults.PageStatus;
    public Dictionary<string, string> PaginationRouteData { get; set; } = new();
    public string PageSearchFormInputId => "search";
    [BindProperty(SupportsGet = true)] public string Id { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public ResultType? SearchResultType { get; set; }
    [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;

    public IPaginatedList<SearchResultServiceModel> SearchResults { get; set; } = PaginatedList<SearchResultServiceModel>.Empty();
    public SearchResultsOverview ResultCount { get; set; } = new();

    public SearchModel(ISearchService searchService, ITrustService trustService, ISchoolService schoolService)
    {
        _trustService = trustService;
        _searchService = searchService;
        _schoolService = schoolService;
        ShowHeaderSearch = false;
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("/Search", new { KeyWords, Id, SearchResultType });
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(Id))
        {
            if (SearchResultType is ResultType.Trust)
            {
                var trust = await _trustService.GetTrustSummaryAsync(Id);

                if (trust != null && string.Equals(trust.Name, KeyWords, StringComparison.CurrentCultureIgnoreCase))
                {
                    return RedirectToPage("/trusts/overview/trustdetails", new { Uid = Id });
                }
            }

            if (SearchResultType is ResultType.School && int.TryParse(Id, out var schoolUrn))
            {
                var school = await _schoolService.GetSchoolSummaryAsync(schoolUrn);
                if (school != null && string.Equals(school.Name, KeyWords, StringComparison.CurrentCultureIgnoreCase))
                {
                    return RedirectToPage("/schools/overview/details", new { urn = schoolUrn });
                }
            }
        }

        var searchResults = await _searchService.GetSearchResultsForPageAsync(KeyWords, PageNumber);
        SearchResults = searchResults.ResultsList;
        ResultCount = searchResults.ResultsOverview;

        PaginationRouteData = new Dictionary<string, string> { { "Keywords", KeyWords ?? string.Empty } };
        return new PageResult();
    }
    
    public async Task<IActionResult> OnGetPopulateAutocompleteAsync()
    {
        var result = (await _searchService.GetSearchResultsForAutocompleteAsync(KeyWords!))
            .Select(result => new AutocompleteEntry(result.Id, result.Address, result.Name, result.ResultType));
        
        return new JsonResult(result);
    }

    public string Title
    {
        get
        {
            if (string.IsNullOrWhiteSpace(KeyWords))
            {
                return "Search";
            }

            if (PageStatus.TotalPages > 1)
            {
                return $"Search (page {PageStatus.PageIndex} of {PageStatus.TotalPages}) - {KeyWords}";
            }

            return $"Search - {KeyWords}";
        }
    }
}
