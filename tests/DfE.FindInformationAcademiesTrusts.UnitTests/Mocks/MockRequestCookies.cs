using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Configuration;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockRequestCookies : IRequestCookieCollection
{
    private Dictionary<string, string> Data { get; } = new();

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

    public bool ContainsKey(string key) => Data.ContainsKey(key);

    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value) => Data.TryGetValue(key, out value);

    public int Count => Data.Count;
    public ICollection<string> Keys => Data.Keys;

    public string? this[string key] => Data[key];

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => Data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
