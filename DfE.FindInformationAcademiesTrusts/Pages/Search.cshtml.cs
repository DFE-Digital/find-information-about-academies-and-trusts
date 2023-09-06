using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : LayoutModel, ISearchFormModel
{
    private readonly ITrustProvider _trustProvider;

    public SearchModel(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    public string InputId => "search";
    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = string.Empty;
    public IEnumerable<Trust> Trusts { get; set; } = Array.Empty<Trust>();

    public async Task OnGetAsync()
    {
        Trusts = await GetTrustsForKeywords();
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
            .Select(trust => new
            {
                address = $"{trust.Address}",
                name = $"{trust.Name}",
                trustId = $"{trust.Ukprn}"
            }));
    }
}
