using DfE.FIAT.Data;
using DfE.FIAT.Web.Pages.Shared;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FIAT.Web.Pages;

public class SearchModel : ContentPageModel, IPageSearchFormModel, IPaginationModel
{
    public record AutocompleteEntry(string Address, string Name, string? TrustId);

    private readonly ITrustSearch _trustSearch;
    private readonly ITrustService _trustService;
    public string PageName => "Search";
    public IPageStatus PageStatus => Trusts.PageStatus;
    public Dictionary<string, string> PaginationRouteData { get; set; } = new();
    public string PageSearchFormInputId => "search";
    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;

    public IPaginatedList<TrustSearchEntry> Trusts { get; set; } = PaginatedList<TrustSearchEntry>.Empty();

    public SearchModel(ITrustService trustService, ITrustSearch trustSearch)
    {
        _trustSearch = trustSearch;
        _trustService = trustService;
        ShowHeaderSearch = false;
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("/Search", new { KeyWords, Uid });
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(Uid))
        {
            var trust = await _trustService.GetTrustSummaryAsync(Uid);
            if (trust != null && string.Equals(trust.Name, KeyWords, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToPage("/Trusts/Overview/TrustDetails", new { Uid });
            }
        }

        Trusts = await _trustSearch.SearchAsync(KeyWords, PageNumber);

        PaginationRouteData = new Dictionary<string, string> { { "Keywords", KeyWords ?? string.Empty } };
        return new PageResult();
    }

    public async Task<IActionResult> OnGetPopulateAutocompleteAsync()
    {
        var autocompleteEntries =
            (await _trustSearch.SearchAutocompleteAsync(KeyWords))
            .Select(trust =>
                new AutocompleteEntry(
                    trust.Address,
                    trust.Name,
                    trust.Uid
                ));

        return new JsonResult(autocompleteEntries);
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
