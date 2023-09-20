using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : PageModel, ISearchFormModel
{
    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = string.Empty;
    public string InputId => "home-search";
}
