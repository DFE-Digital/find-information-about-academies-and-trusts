using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel : LayoutModel
{
    private readonly ITrustProvider _trustProvider;

    public DetailsModel(ITrustProvider trustProvider)
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
