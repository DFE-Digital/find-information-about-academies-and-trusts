using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : PageModel
{
    private ITrustSearch _trustSearch;


    public SearchModel(ITrustSearch trustSearch)
    {
        _trustSearch = trustSearch;
    }

    [BindProperty(SupportsGet = true)] public string? Query { get; set; }
    public IEnumerable<Trust> Trusts { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrEmpty(Query))
        {
            Trusts = await _trustSearch.SearchAsync(Query);
        }
    }
}
