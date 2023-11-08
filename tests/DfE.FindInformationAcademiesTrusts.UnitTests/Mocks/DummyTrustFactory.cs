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

    public static Trust GetDummyTrust(string uid)
    {
        return new Trust(uid, $"Trust {uid}", "test", "test", "test", "test", new DateTime(), "test", "test");
    }
}
