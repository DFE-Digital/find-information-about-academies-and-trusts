using DfE.FindInformationAcademiesTrusts.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages.Shared;

public abstract class BasePageModel : PageModel
{
    private readonly bool _showHeaderSearch = true;

    public bool ShowHeaderSearch
    {
        get => _showHeaderSearch && User.HasAccessToFiat();
        init => _showHeaderSearch = value;
    }

    [BindProperty(SupportsGet = true)] public string? KeyWords { get; set; } = string.Empty;
}
