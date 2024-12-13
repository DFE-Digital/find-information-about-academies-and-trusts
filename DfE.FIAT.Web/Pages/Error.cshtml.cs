using DfE.FIAT.Web.Pages.Shared;
using Microsoft.AspNetCore.Diagnostics;

namespace DfE.FIAT.Web.Pages;

public class ErrorModel(IHttpContextAccessor httpContextAccessor) : ContentPageModel
{
    public bool Is404Result { get; set; }
    public string OriginalPathAndQuery { get; set; } = "Unknown";

    public void OnGet(string statusCode)
    {
        if (statusCode == "404")
        {
            Is404Result = true;

            var notFoundData = httpContextAccessor.HttpContext!.Features.Get<IStatusCodeReExecuteFeature>();
            if (notFoundData is not null)
            {
                OriginalPathAndQuery =
                    $"{httpContextAccessor.HttpContext!.Request.Host}{notFoundData.OriginalPath}{notFoundData.OriginalQueryString}";
            }
        }

        ShowBreadcrumb = Is404Result;
    }
}
