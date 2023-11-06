using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

[ValidateAntiForgeryToken]
public class SearchModel : PageModel, ISearchFormModel
{
    public record AutocompleteEntry(string Address, string Name, string? TrustId);

    private readonly ITrustProvider _trustProvider;
    private readonly ITrustSearch _trustSearch;

    public SearchModel(ITrustProvider trustProvider, ITrustSearch trustSearch)
    {
        _trustProvider = trustProvider;
        _trustSearch = trustSearch;
    }

    public string InputId => "search";
    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public int SearchPageNumber { get; set; } = 1;

    public IPaginatedList<TrustSearchEntry> Trusts { get; set; } = new PaginatedList<TrustSearchEntry>();


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
        return new PageResult();
    }

    private async Task<IPaginatedList<TrustSearchEntry>> GetTrustsForKeywords()
    {
        return !string.IsNullOrEmpty(KeyWords)
            ? await _trustSearch.SearchAsync(KeyWords, SearchPageNumber)
            : new PaginatedList<TrustSearchEntry>();
    }

    public async Task<IActionResult> OnGetPopulateAutocompleteAsync()
    {
        return new JsonResult((await GetTrustsForKeywords()).ToArray()
            .Select(trust =>
                new AutocompleteEntry(
                    trust.Address,
                    trust.Name,
                    trust.Uid
                )));
    }
}
