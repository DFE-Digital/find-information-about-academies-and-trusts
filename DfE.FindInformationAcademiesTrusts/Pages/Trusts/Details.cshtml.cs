using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class Details : LayoutModel
{
    [BindProperty(SupportsGet = true)] public string? Ukprn { get; set; }


    public void OnGet()
    {
    }
}
