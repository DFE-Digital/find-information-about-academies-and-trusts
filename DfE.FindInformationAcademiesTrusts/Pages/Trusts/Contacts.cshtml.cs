using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class ContactsModel : LayoutModel, ITrustsAreaModel
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
