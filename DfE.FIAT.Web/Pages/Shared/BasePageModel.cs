using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FIAT.Web.Pages.Shared;

public abstract class BasePageModel : PageModel
{
    public bool ShowHeaderSearch { get; init; } = true;
    [BindProperty(SupportsGet = true)] public string? KeyWords { get; set; } = string.Empty;
}
