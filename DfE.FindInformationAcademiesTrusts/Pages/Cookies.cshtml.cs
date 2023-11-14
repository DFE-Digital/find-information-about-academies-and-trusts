using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class CookiesModel : PageModel
{
    public bool PreferencesSet { get; set; }
    public string? ReturnPath { get; set; }

    public ActionResult OnGet(bool? consent, string returnUrl)
    {
        ReturnPath = returnUrl;

        if (consent == true) AcceptCookies();

        if (consent == false) RejectCookies();

        if (returnUrl == null || consent == null) return Page();

        return Redirect(returnUrl);
    }

    public ActionResult OnPost(bool? consent, string returnUrl)
    {
        ReturnPath = returnUrl;

        if (consent == true) AcceptCookies();

        if (consent == false) RejectCookies();

        PreferencesSet = true;

        return Page();
    }

    private void AcceptCookies()
    {
        HttpContext.Session.SetInt32("CookieStatus", (int) CookieStatusEnums.CookieStatus.Accepted);
    }

    private void RejectCookies()
    {
        Response.Cookies.Delete("ai_session", new CookieOptions { Path = "/" });
        Response.Cookies.Delete("ai_user", new CookieOptions { Path = "/" });
        HttpContext.Session.SetInt32("CookieStatus", (int) CookieStatusEnums.CookieStatus.Rejected);
    }
}
