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
        if (!string.IsNullOrEmpty(KeyWords))
        {
            Trusts = await _trustProvider.GetTrustsByNameAsync(KeyWords);
        }
    }
}
