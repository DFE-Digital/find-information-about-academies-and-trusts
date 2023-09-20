using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class ContactsModel : PageModel, ITrustsAreaModel
{
    private readonly ITrustProvider _trustProvider;

    public ContactsModel(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    [BindProperty(SupportsGet = true)] public string Ukprn { get; set; } = "";
    public Trust Trust { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var trust = await _trustProvider.GetTrustByUkprnAsync(Ukprn);
        Trust = trust;
        return Page();
    }
}
