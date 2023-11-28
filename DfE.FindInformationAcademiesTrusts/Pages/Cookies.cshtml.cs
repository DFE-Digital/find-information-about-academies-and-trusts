using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class CookiesModel : PageModel
{
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
                Consent = CookiesHelper.OptionalCookiesAreAccepted(_httpContextAccessor.HttpContext!, TempData);
            }

            return Page();
        }

        return LocalRedirect(ReturnPath!);
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
            _httpContextAccessor.HttpContext!.Response.Cookies.Append(CookiesHelper.ConsentCookieName,
                Consent.Value.ToString(), cookieOptions);
            TempData[CookiesHelper.CookieChangedTempDataName] = true;
        }

        if (Consent is false)
        {
            var optionalCookieMatches = new[] { "ai_session", "ai_user" };
            foreach (var cookie in optionalCookieMatches)
            {
                if (_httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(cookie))
                {
                    _httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookie);
                }
            }

            TempData[CookiesHelper.DeleteCookieTempDataName] = true;
        }
    }

    private void ValidateReturnPath()
    {
        // Expect to be a path eg starts with a slash
        // If its not a path it didn't come from us
        // Treat it as incorrect and remove it
        if (ReturnPath.IsNullOrEmpty() || !ReturnPath!.StartsWith("/") || !Url.IsLocalUrl(ReturnPath))
        {
            ReturnPath = "/";
        }
    }


    private bool CookiesPreferencesHaveBeenSet()
    {
        return _httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(CookiesHelper.ConsentCookieName);
    }
}
