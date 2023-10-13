using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class OverviewModel : PageModel, ITrustsAreaModel
{
    private readonly ITrustProvider _trustProvider;

    public OverviewModel(ITrustProvider trustProvider)
    {
        _trustProvider = trustProvider;
    }

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public Trust Trust { get; set; } = default!;
    public string PageName => "Overview";
    public string Section => ViewConstants.AboutTheTrustSectionName;


    public async Task<IActionResult> OnGetAsync()
    {
        var trust = await _trustProvider.GetTrustByGroupUidAsync(Uid);

        if (trust == null)
        {
            return new NotFoundResult();
        }

        Trust = trust;
        return Page();
    }
}
