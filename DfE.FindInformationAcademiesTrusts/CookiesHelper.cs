using DfE.FindInformationAcademiesTrusts.Configuration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DfE.FindInformationAcademiesTrusts;

public static class CookiesHelper
{
    public const string DeleteCookieTempDataName = "DeleteCookie";
    public const string CookieChangedTempDataName = "CookieResponse";
    public const string ReturnPathQuery = "returnPath";

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

        return context.Request.Cookies.ContainsKey(FiatCookies.CookieConsent) &&
               bool.Parse(context.Request.Cookies[FiatCookies.CookieConsent]!);
    }

    public static string ReturnPath(HttpContext context)
    {
        return string.IsNullOrWhiteSpace(context.Request.Query[ReturnPathQuery])
            ? context.Request.Path + context.Request.QueryString
            : context.Request.Query[ReturnPathQuery].ToString();
    }

    public static bool ShowCookieBanner(HttpContext context, ITempDataDictionary tempData)
    {
        return !context.Request.Cookies.ContainsKey(FiatCookies.CookieConsent) &&
               tempData[DeleteCookieTempDataName] is null;
    }
}
