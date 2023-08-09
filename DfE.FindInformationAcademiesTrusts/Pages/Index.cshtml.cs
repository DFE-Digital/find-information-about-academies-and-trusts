using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : LayoutModel
{
    public IndexModel()
    {
        UsePageWidthContainer = false;
    }

    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = "";
}
