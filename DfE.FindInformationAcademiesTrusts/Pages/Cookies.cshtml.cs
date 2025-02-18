using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class CookiesModel(IHttpContextAccessor httpContextAccessor) : ContentPageModel
{
    private string? _returnPath;
    public bool DisplayCookieChangedMessageOnCookiesPage { get; set; }
    [BindProperty(SupportsGet = true)] public bool? Consent { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnPath
    {
        get => _returnPath;
        set
        {
            // Sanitise to ensure malicious/bad urls can't be passed in and redirected to
            if (string.IsNullOrWhiteSpace(value) || !Url.IsLocalUrl(value))
            {
                // If it's not a valid local url it didn't come from us, treat it as incorrect and remove it
                _returnPath = "/";
            }
            else
            {
                _returnPath = value;
            }
        }
    }

    public ActionResult OnGet()
    {
        if (CookiesPreferencesHaveBeenSet())
        {
            Consent = CookiesHelper.OptionalCookiesAreAccepted(httpContextAccessor.HttpContext!, TempData);
        }

        return Page();
    }

    public ActionResult OnPost()
    {
        ApplyCookieConsent();

        DisplayCookieChangedMessageOnCookiesPage = true;

        return Page();
    }

    public ActionResult OnPostFromBanner()
    {
        ApplyCookieConsent();

        return LocalRedirect(ReturnPath ?? "/");
    }

    private void ApplyCookieConsent()
    {
        if (Consent.HasValue)
        {
            CookieOptions cookieOptions = new()
                { Expires = DateTime.UtcNow.AddYears(1), Secure = true, HttpOnly = true };
            httpContextAccessor.HttpContext!.Response.Cookies.Append(FiatCookies.CookieConsent,
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

        cookiesToDelete.ForEach(cookie => httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookie));
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
            httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookie);
            httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookie,
                new CookieOptions { Domain = ".education.gov.uk" });
        });
    }

    private List<string> GetMatchingCookies(string[] optionalCookiePrefixes)
    {
        var currentCookies = httpContextAccessor.HttpContext!.Request.Cookies.Keys;

        var cookiesToDelete = currentCookies
            .Where(currentCookie => optionalCookiePrefixes.Any(prefix => currentCookie.StartsWith(prefix)))
            .ToList();

        return cookiesToDelete;
    }

    private bool CookiesPreferencesHaveBeenSet()
    {
        return httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(FiatCookies.CookieConsent);
    }
}
