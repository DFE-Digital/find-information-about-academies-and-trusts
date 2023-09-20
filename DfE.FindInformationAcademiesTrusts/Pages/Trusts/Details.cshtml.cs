using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel : PageModel
{
    private readonly ITrustProvider _trustProvider;

    public DetailsModel(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    [BindProperty(SupportsGet = true)] public string Ukprn { get; set; } = "";
    public Trust Trust { get; set; } = default!;
    public string PageName => "Details";
    public string Section => ViewConstants.AboutTheTrustSectionName;


    public async Task<IActionResult> OnGetAsync()
    {
        var trust = await _trustProvider.GetTrustByUkprnAsync(Ukprn);
        Trust = trust;
        return Page();
    }
}
