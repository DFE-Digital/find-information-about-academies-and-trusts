using DfE.FindInformationAcademiesTrusts.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DfE.FindInformationAcademiesTrusts.Authorization;

public class FiatCookieAuthenticationEvents : CookieAuthenticationEvents
{
    public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
    {
        // Users may be redirected to Access Denied because the login cookie stored in their browser contains
        // stale information about their roles
        // We can force an update of their role claims by removing this login cookie

        if (AlreadyRetriedLogin(context.Options.CookieManager, context.HttpContext))
            return base.RedirectToAccessDenied(context);

        return RetryLogin(context);
    }

    private Task RetryLogin(RedirectContext<CookieAuthenticationOptions> context)
    {
        var httpContext = context.HttpContext;
        var cookieManager = context.Options.CookieManager;

        RemoveOldLoginCookie(cookieManager, httpContext);
        AddLoginRetriedRecentlyCookie(cookieManager, httpContext);

        return base.RedirectToReturnUrl(context);
    }

    private static void RemoveOldLoginCookie(ICookieManager cookieManager, HttpContext httpContext)
    {
        if (cookieManager.GetRequestCookie(httpContext, FiatCookies.Login) is not null)
        {
            cookieManager.DeleteCookie(
                httpContext,
                FiatCookies.Login,
                new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });
        }
    }

    private static void AddLoginRetriedRecentlyCookie(ICookieManager cookieManager, HttpContext httpContext)
    {
        cookieManager.AppendResponseCookie(
            httpContext,
            FiatCookies.LoginRetriedRecently,
            "true",
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(1),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
    }

    private static bool AlreadyRetriedLogin(ICookieManager cookieManager, HttpContext httpContext)
    {
        return cookieManager.GetRequestCookie(httpContext, FiatCookies.LoginRetriedRecently) is not null;
    }
}
