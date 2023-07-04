using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Shared;

public class LayoutModel : PageModel
{
    public bool UsePageWidthContainer { get; set; } = true;

    public string? GetPageWidthClass(string attribute)
    {
        return UsePageWidthContainer ? attribute : null;
    }
}
