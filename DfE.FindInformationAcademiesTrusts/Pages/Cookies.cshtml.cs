using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class CookiesModel : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public bool PreferencesSet { get; set; }
    public string? ReturnPath { get; set; }

    public CookiesModel(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ActionResult OnGet(bool? consent, string returnUrl)
    {
        ReturnPath = returnUrl;

        if (consent == true) AcceptCookies();

        if (consent == false) RejectCookies();

        if (returnUrl.IsNullOrEmpty() || consent == null) return Page();

        return Redirect(returnUrl);
    }

    public ActionResult OnPost(bool? consent, string returnUrl)
    {
        ReturnPath = returnUrl;

        if (consent == true) AcceptCookies();

        if (consent == false) RejectCookies();

        DisplayCookieChangedMessageOnCookiesPage = true;

        return Page();
    }

    private void AcceptCookies()
    {
        _httpContextAccessor.HttpContext?.Session.SetInt32("CookieStatus",
            (int)CookieStatusEnums.CookieStatus.Accepted);
    }

    private void RejectCookies()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("ai_session",
            new CookieOptions { Path = "/", Secure = true, HttpOnly = true });
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("ai_user",
            new CookieOptions { Path = "/", Secure = true, HttpOnly = true });
        _httpContextAccessor.HttpContext?.Session.SetInt32("CookieStatus",
            (int)CookieStatusEnums.CookieStatus.Rejected);
    }
}
