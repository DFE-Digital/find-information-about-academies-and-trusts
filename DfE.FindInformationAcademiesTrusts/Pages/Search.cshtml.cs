using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : LayoutModel, ISearchFormModel
{
    public record AutocompleteEntry(string Address, string Name, string? TrustId);

    private readonly ITrustProvider _trustProvider;

    public SearchModel(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    public string InputId => "search";
    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public string TrustId { get; set; } = string.Empty;
    public IEnumerable<Trust> Trusts { get; set; } = Array.Empty<Trust>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(TrustId))
        {
            var trust = await _trustProvider.GetTrustByUkprnAsync(TrustId);
            if (string.Equals(trust.Name, KeyWords, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToPage("/Trusts/Details", new { Ukprn = TrustId });
            }
        }

        Trusts = await GetTrustsForKeywords();
        return new PageResult();
    }

    private async Task<IEnumerable<Trust>> GetTrustsForKeywords()
    {
        return !string.IsNullOrEmpty(KeyWords)
            ? await _trustProvider.GetTrustsByNameAsync(KeyWords)
            : Array.Empty<Trust>();
    }

    public async Task<IActionResult> OnGetPopulateAutocompleteAsync()
    {
        return new JsonResult((await GetTrustsForKeywords())
            .Select(trust =>
                new AutocompleteEntry(
                    trust.Address,
                    trust.Name,
                    trust.Ukprn
                )));
    }
}
