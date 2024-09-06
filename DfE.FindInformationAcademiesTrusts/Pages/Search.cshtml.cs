using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : ContentPageModel, IPageSearchFormModel, IPaginationModel
{
    public record AutocompleteEntry(string Address, string Name, string? TrustId);

    private readonly ITrustSearch _trustSearch;
    private readonly ITrustService _trustService;
    public string PageName { get; } = "Search";
    public IPageStatus PageStatus { get; set; }
    public Dictionary<string, string> PaginationRouteData { get; set; } = new();
    public string PageSearchFormInputId => "search";
    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;

    public IPaginatedList<TrustSearchEntry> Trusts { get; set; } =
        PaginatedList<TrustSearchEntry>.Empty();

    public SearchModel(ITrustService trustService, ITrustSearch trustSearch)
    {
        _trustSearch = trustSearch;
        _trustService = trustService;
        PageStatus = Trusts.PageStatus;
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
