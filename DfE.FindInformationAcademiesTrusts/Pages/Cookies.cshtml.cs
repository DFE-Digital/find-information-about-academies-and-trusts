using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.IdentityModel.Tokens;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class CookiesModel : PageModel
{
    public static string ConsentCookieName = ".FindInformationAcademiesTrust.CookieConsent";
    public static string DeleteCookieTempDataName = "DeleteCookie";
    public static string CookieChangedTempDataName = "CookieResponse";
    private readonly IHttpContextAccessor _httpContextAccessor;
    public bool DisplayCookieChangedMessageOnCookiesPage { get; set; }
    [BindProperty(SupportsGet = true)] public string? ReturnPath { get; set; }
    [BindProperty(SupportsGet = true)] public bool? Consent { get; set; }

    public CookiesModel(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Used to load the cookies page or to set cookie preferences from the cookie banner
    /// </summary>
    /// <returns>The cookies page if we are not setting the cookie preference, otherwise redirect to the redirect path (from the cookie banner)</returns>
    public ActionResult OnGet()
    {
        ValidateReturnPath();

        ApplyCookieConsent();

        if (ReturnPath.IsNullOrEmpty() || Consent is null)
        {
            if (CookiesPreferencesHaveBeenSet())
            {
                Consent = OptionalCookiesAreAccepted(_httpContextAccessor.HttpContext!, TempData);
            }

            return Page();
        }

        return Redirect(ReturnPath!);
    }

    public ActionResult OnPost()
    {
        ValidateReturnPath();

        ApplyCookieConsent();

        DisplayCookieChangedMessageOnCookiesPage = true;

        return Page();
    }

    private void ApplyCookieConsent()
    {
        if (Consent.HasValue)
        {
            CookieOptions cookieOptions = new()
                { Expires = DateTime.UtcNow.AddYears(1), Secure = true, HttpOnly = true };
            Response.Cookies.Append(ConsentCookieName, Consent.Value.ToString(), cookieOptions);
            TempData[CookieChangedTempDataName] = true;
        }

        if (Consent is false)
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                var optionalCookieMatches = new[] { "ai_session", "ai_user" };
                foreach (var match in optionalCookieMatches)
                {
                    if (match == cookie)
                    {
                        Response.Cookies.Delete(cookie);
                    }
                }
            }

            TempData[DeleteCookieTempDataName] = true;
        }
    }

    private void ValidateReturnPath()
    {
        // Expect to be a path eg starts with a slash
        // If its not a path it didn't come from us
        // Treat it as incorrect and remove it
        if (ReturnPath.IsNullOrEmpty() || !ReturnPath!.StartsWith("/"))
        {
            ReturnPath = "/";
        }
    }

    /// <summary>
    /// Returns whether the user has accepted or rejected optional cookies
    /// </summary>
    /// <param name="context">The current HTTP context</param>
    /// <param name="tempData">The temporary data returned from the response</param>
    /// <returns></returns>
    public static bool OptionalCookiesAreAccepted(HttpContext context, ITempDataDictionary tempData)
    {
        if (tempData[DeleteCookieTempDataName] is not null)
        {
            return false;
        }

        return context.Request.Cookies.ContainsKey(ConsentCookieName) &&
               bool.Parse(context.Request.Cookies[ConsentCookieName]!);
    }

    private bool CookiesPreferencesHaveBeenSet()
    {
        return _httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(ConsentCookieName);
    }
}
