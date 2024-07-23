using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Shared;

public class BasePageModel : PageModel
{
    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = string.Empty;
}
