using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : LayoutModel, ISearchFormModel
{
    public IndexModel()
    {
        UsePageWidthContainer = false;
    }

    [BindProperty(SupportsGet = true)] public string KeyWords { get; set; } = "";
    public string InputId => "home-search";
}
