using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : LayoutModel
{
    private readonly ITrustProvider _trustProvider;

    public SearchModel(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    [BindProperty(SupportsGet = true)] public string? KeyWords { get; set; }
    public IEnumerable<Trust> Trusts { get; set; } = Array.Empty<Trust>();

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrEmpty(KeyWords))
        {
            Trusts = await _trustProvider.GetTrustsByNameAsync(KeyWords);
        }
    }
}
