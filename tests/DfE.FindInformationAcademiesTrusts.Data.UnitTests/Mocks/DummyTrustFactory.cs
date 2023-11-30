namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

public class DummyTrustFactory
{
    private int _numberTrustsGenerated;
    private const string OpenStatus = "Open";
    public const string ClosedStatus = "Closed";

    public Trust GetDummyTrust()
    {
        _numberTrustsGenerated++;
        return GetDummyTrust(_numberTrustsGenerated.ToString("0000"));
    }

    public static Trust GetDummyMultiAcademyTrust(string uid, string companiesHouseNumber = "test",
        string status = OpenStatus)
    {
        return GetDummyTrust(uid, "Multi-academy trust", companiesHouseNumber, status: status);
    }

    public static Trust GetDummySingleAcademyTrust(string uid, Academy? academy = null, string status = OpenStatus)
    {
        var academies = academy is not null ? new[] { academy } : Array.Empty<Academy>();
        return GetDummyTrust(uid, "Single-academy trust", academies: academies);
    }

    public static Trust GetDummyTrust(string uid, string type = "test", string companiesHouseNumber = "test",
        Academy[]? academies = null, string status = OpenStatus)
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
            status
        );
    }
}
