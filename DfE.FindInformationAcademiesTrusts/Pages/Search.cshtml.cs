using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : LayoutModel
{
    private readonly ITrustSearch _trustSearch;


    public SearchModel(ITrustSearch trustSearch)
    {
        _trustSearch = trustSearch;
    }

    [BindProperty(SupportsGet = true)] public string? Query { get; set; }
    public IEnumerable<Trust> Trusts { get; set; } = Array.Empty<Trust>();

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrEmpty(Query))
        {
            Trusts = await _trustSearch.SearchAsync(Query);
        }
    }
}
