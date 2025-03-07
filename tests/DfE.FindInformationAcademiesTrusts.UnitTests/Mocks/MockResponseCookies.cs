using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockResponseCookies
{
    public IResponseCookies Cookies { get; } = Substitute.For<IResponseCookies>();

    public void VerifySecureCookieAdded(string key, string value)
    {
        Cookies.Received(1).Append(key, value, Arg.Is<CookieOptions>(c => c.Secure == true && c.HttpOnly == true));
    }

    public void VerifyCookieDeleted(string key)
    {
        Cookies.Received(1).Delete(key);
    }

    public void VerifyCookieDeleted(string key, CookieOptions options)
    {
        Cookies.Received(1).Delete(key, Arg.Is<CookieOptions>(cookieOptions =>
            cookieOptions.HttpOnly == options.HttpOnly
            && cookieOptions.IsEssential == options.IsEssential
            && cookieOptions.SameSite == options.SameSite
            && cookieOptions.Secure == options.Secure
        ));
    }

    public void VerifyNoCookiesDeleted()
    {
        Cookies.DidNotReceive().Delete(Arg.Any<string>());
    }
}
