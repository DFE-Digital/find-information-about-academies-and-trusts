using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class TrustsAreaModel : PageModel, ITrustsAreaModel
{
    private readonly ITrustProvider _trustProvider;

    public TrustsAreaModel(ITrustProvider trustProvider, string pageName)
    {
        _trustProvider = trustProvider;
        PageName = pageName;
    }

    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = "";
    public Trust Trust { get; set; } = default!;
    public string PageName { get; init; }
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
