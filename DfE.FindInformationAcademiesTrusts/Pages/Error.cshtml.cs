using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class ErrorModel : PageModel
{
    public bool Is404Result { get; set; }

    public void OnGet(string statusCode)
    {
        if (statusCode == "404")
        {
            Is404Result = true;
        }
    }
}
