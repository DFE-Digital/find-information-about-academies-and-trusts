using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : PageModel, ISearchFormModel, IPaginationModel
{
    public record AutocompleteEntry(string Address, string Name, string? TrustId);

    private readonly ITrustProvider _trustProvider;
    private readonly ITrustSearch _trustSearch;
    public string PageName { get; } = "Search";
    public IPageStatus PageStatus { get; set; }
    public Dictionary<string, string> PaginationRouteData { get; set; } = new();
    public string InputId => "search";
    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;

    public IPaginatedList<TrustSearchEntry> Trusts { get; set; } =
        PaginatedList<TrustSearchEntry>.Empty();

    public SearchModel(ITrustProvider trustProvider, ITrustSearch trustSearch)
    {
        _trustProvider = trustProvider;
        _trustSearch = trustSearch;
        PageStatus = Trusts.PageStatus;
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("/Search", new { KeyWords, Uid });
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(Uid))
        {
            var trust = await _trustProvider.GetTrustByUidAsync(Uid);
            if (trust != null && string.Equals(trust.Name, KeyWords, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToPage("/Trusts/Details", new { Uid });
            }
        }

        Trusts = await GetTrustsForKeywords();

        PageStatus = Trusts.PageStatus;
        PaginationRouteData = new Dictionary<string, string> { { "Keywords", KeyWords } };
        return new PageResult();
    }

    private async Task<IPaginatedList<TrustSearchEntry>> GetTrustsForKeywords()
    {
        return !string.IsNullOrEmpty(KeyWords)
            ? await _trustSearch.SearchAsync(KeyWords, PageNumber)
            : PaginatedList<TrustSearchEntry>.Empty();
    }

    public async Task<IActionResult> OnGetPopulateAutocompleteAsync()
    {
        return new JsonResult((await GetTrustsForKeywords())
            .Select(trust =>
                new AutocompleteEntry(
                    trust.Address,
                    trust.Name,
                    trust.Uid
                )));
    }

    public string Title()
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
