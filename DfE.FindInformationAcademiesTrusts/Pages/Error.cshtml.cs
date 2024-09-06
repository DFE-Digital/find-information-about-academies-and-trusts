using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Diagnostics;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class ErrorModel(IHttpContextAccessor httpContextAccessor) : ContentPageModel
{
    public bool Is404Result { get; set; }
    public string OriginalPathAndQuery { get; set; } = "Unknown";

    public void OnGet(string statusCode)
    {
        switch (statusCode)
        {
            case "404":
                Is404Result = true;

                var notFoundData = httpContextAccessor.HttpContext!.Features.Get<IStatusCodeReExecuteFeature>();
                if (notFoundData is not null)
                {
                    OriginalPathAndQuery =
                        $"{httpContextAccessor.HttpContext!.Request.Host}{notFoundData.OriginalPath}{notFoundData.OriginalQueryString}";
                }

                break;

            case "500":
                ShowBreadcrumb = false;

                break;
        }
    }
}
