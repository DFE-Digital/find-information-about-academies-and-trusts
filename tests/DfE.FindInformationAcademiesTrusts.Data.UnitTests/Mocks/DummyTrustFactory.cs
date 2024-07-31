namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

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
        var academies = academy is not null ? new[] { academy } : [];
        return GetDummyTrust(uid, "Single-academy trust", academies: academies);
    }

    public static Trust GetDummyTrust(string uid, string type = "test", string companiesHouseNumber = "test",
        Academy[]? academies = null, Governor[]? governors = null)
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
            academies ?? [],
            governors ?? [],
            new Person("Present Trm", "trm@test.com"),
            new Person("Present Sfsolead", "Sfsolead@test.com"),
            "Open"
        );
    }
}
