using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class DummyTrustFactory
{
    private int _numberTrustsGenerated;

    public Trust GetDummyTrust()
    {
        _numberTrustsGenerated++;
        return GetDummyTrust(_numberTrustsGenerated.ToString("0000"));
    }

    public static Trust GetDummyMultiAcademyTrust(string uid, string companiesHouseNumber = "test")
    {
        return GetDummyTrust(uid, "Multi-academy trust", companiesHouseNumber);
    }

    public static Trust GetDummySingleAcademyTrust(string uid, Academy? academy = null)
    {
        var academies = academy is not null ? new[] { academy } : Array.Empty<Academy>();
        return GetDummyTrust(uid, "Single-academy trust", academies: academies);
    }

    public static Trust GetDummyTrust(string uid, string type = "test", string companiesHouseNumber = "test",
        Academy[]? academies = null)
    {
        return new Trust(uid,
            $"Trust {uid}",
            "test",
            "test",
            type,
            "test",
            new DateTime(),
            companiesHouseNumber,
            "test",
            academies ?? Array.Empty<Academy>(),
            Array.Empty<Governor>(),
            null,
            null,
            "Open"
        );
    }
}
