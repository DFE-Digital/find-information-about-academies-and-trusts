using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class CookiesModel : ContentPageModel
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

        if (string.IsNullOrWhiteSpace(ReturnPath) || Consent is null)
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
            DeleteAppInsightsCookies();
            DeleteGoogleAnalyticsCookies();

            TempData[CookiesHelper.DeleteCookieTempDataName] = true;
        }
    }

    private void DeleteAppInsightsCookies()
    {
        var optionalCookiePrefixes = new[] { "ai_session", "ai_user", "ai_authUser" };
        var cookiesToDelete = GetMatchingCookies(optionalCookiePrefixes);

        cookiesToDelete.ForEach(cookie => _httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookie));
    }

    private void DeleteGoogleAnalyticsCookies()
    {
        var optionalCookiePrefixes = new[] { "_ga", "_gid" };
        var cookiesToDelete = GetMatchingCookies(optionalCookiePrefixes);

        cookiesToDelete.ForEach(cookie =>
        {
            // Need to delete cookies that exists outside of our current host
            // Can't delete cookies unless the domain is specified, by default it will be the current host
            // Issue was it worked on localhost but not on dev, test or prod, because google analytics cookies are set on a different domain
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookie);
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookie,
                new CookieOptions { Domain = ".education.gov.uk" });
        });
    }

    private List<string> GetMatchingCookies(string[] optionalCookiePrefixes)
    {
        var currentCookies = _httpContextAccessor.HttpContext!.Request.Cookies.Keys;

        var cookiesToDelete = currentCookies
            .Where(currentCookie => optionalCookiePrefixes.Any(prefix => currentCookie.StartsWith(prefix)))
            .ToList();

        return cookiesToDelete;
    }

    private void ValidateReturnPath()
    {
        // Expect to be a path eg starts with a slash
        // If its not a path it didn't come from us
        // Treat it as incorrect and remove it
        if (string.IsNullOrWhiteSpace(ReturnPath) || !Url.IsLocalUrl(ReturnPath))
        {
            ReturnPath = "/";
        }
    }


    private bool CookiesPreferencesHaveBeenSet()
    {
        return _httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(CookiesHelper.ConsentCookieName);
    }
}
