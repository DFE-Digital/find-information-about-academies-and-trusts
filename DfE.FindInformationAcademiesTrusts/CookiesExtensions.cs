using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class CookiesExtensions
{
    public const string ConsentCookieName = ".FindInformationAcademiesTrust.CookieConsent";
    public const string DeleteCookieTempDataName = "DeleteCookie";
    public const string CookieChangedTempDataName = "CookieResponse";

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

    public static string ReturnPath(HttpContext context)
    {
        return string.IsNullOrWhiteSpace(context.Request.Query["returnPath"])
            ? context.Request.Path + context.Request.QueryString
            : context.Request.Query["returnPath"].ToString();
    }

    public static bool ShowCookieBanner(HttpContext context, ITempDataDictionary tempData)
    {
        return !context.Request.Cookies.ContainsKey(ConsentCookieName) &&
               tempData["DeleteCookies"] is null;
    }
}
