using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockResponseCookies : Mock<IResponseCookies>
{
    public void VerifySecureCookieAdded(string key, string value)
    {
        Verify(m => m.Append(key, value,
            It.Is<CookieOptions>(c => c.Secure == true && c.HttpOnly == true)), Times.Once);
    }

    public void VerifyCookieDeleted(string key)
    {
        Verify(m => m.Delete(key), Times.Once);
    }

    public void VerifyCookieDeleted(string key, CookieOptions options)
    {
        Verify(m => m.Delete(key, It.Is<CookieOptions>(cookieOptions =>
                cookieOptions.HttpOnly == options.HttpOnly
                && cookieOptions.IsEssential == options.IsEssential
                && cookieOptions.SameSite == options.SameSite
                && cookieOptions.Secure == options.Secure
            )),
            Times.Once);
    }

    public void VerifyNoCookiesDeleted()
    {
        Verify(m => m.Delete(It.IsAny<string>()), Times.Exactly(0));
    }
}
