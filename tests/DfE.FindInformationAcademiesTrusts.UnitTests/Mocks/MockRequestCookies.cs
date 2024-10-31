using DfE.FindInformationAcademiesTrusts.Configuration;
using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockRequestCookies : Mock<IRequestCookieCollection>
{
    public Dictionary<string, string> Data { get; } = new();

    public MockRequestCookies()
    {
        Setup(m => m.Keys).Returns(Data.Keys);
        Setup(m => m.ContainsKey(It.IsAny<string>())).Returns((string key) => Data.ContainsKey(key));
        Setup(m => m[It.IsAny<string>()]).Returns((string key) => Data.TryGetValue(key, out var value) ? value : null);
    }

    public void SetupConsentCookie(bool? accepted)
    {
        if (accepted is true)
        {
            SetupAcceptedCookie();
        }
        else if (accepted is false)
        {
            SetupRejectedCookie();
        }
    }

    public void SetupAcceptedCookie()
    {
        Data.Add(FiatCookies.CookieConsent, "True");
    }

    public void SetupRejectedCookie()
    {
        Data.Add(FiatCookies.CookieConsent, "False");
    }

    public void SetupOptionalCookies()
    {
        Data.Add("ai_user", "True");
        Data.Add("ai_session", "True");
        Data.Add("_gid", "True");
        Data.Add("_ga", "True");
    }
}
