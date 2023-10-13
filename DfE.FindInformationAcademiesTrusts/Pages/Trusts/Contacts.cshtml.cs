using DfE.FindInformationAcademiesTrusts.Data;
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

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public Trust Trust { get; set; } = default!;
    public string PageName => "Contacts";
    public string Section => ViewConstants.AboutTheTrustSectionName;

    public async Task<IActionResult> OnGetAsync()
    {
        var trust = await _trustProvider.GetTrustByUidAsync(Uid);

        if (trust == null)
        {
            return new NotFoundResult();
        }

        Trust = trust;
        return Page();
    }
}
