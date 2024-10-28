using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

[AllowAnonymous]
public class NoAccessModel : ContentPageModel
{
    private const string RetryingLogin = "RetryingLogin";

    public ActionResult OnGet(string? returnUrl)
    {
        if (User.HasAccessToFiat())
        {
            return Url.IsLocalUrl(returnUrl) ? LocalRedirect(returnUrl) : Page();
        }

        // Users may be redirected to Access Denied because the login cookie stored in their browser contains
        // stale information about their roles
        // We can force an update of their role claims by removing this login cookie and redirecting them 
        // back to their intended destination
        if (NotRetriedLoginYet())
        {
            RemoveLoginCookie();

            // We use temp data to ensure we don't end up in a redirect loop for genuine unauthorised users
            TempData.Add(RetryingLogin, "true");

            return Url.IsLocalUrl(returnUrl) ? LocalRedirect(returnUrl) : Page();
        }

        TempData.Remove(RetryingLogin);
        return Page();
    }

    private bool NotRetriedLoginYet()
    {
        return !TempData.ContainsKey(RetryingLogin);
    }

    private void RemoveLoginCookie()
    {
        if (Request.Cookies.ContainsKey(FiatCookies.Login))
        {
            Response.Cookies.Delete(FiatCookies.Login,
                new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });
        }
    }
}
